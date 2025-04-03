﻿using System.Text.Json.Nodes;
using Newtonsoft.Json;
using RestSharp;
using SkiaSharp;

const string steam64Id = "76561199388500493";
const string output = "D:";

var mapping = new Dictionary<int, MappingRecord>()
{
    { 4999, new MappingRecord("common", "#B1C3D9") },
    { 9999, new MappingRecord("uncommon", "#5E98D7") },
    { 14999, new MappingRecord("rare", "#4B69FF") },
    { 19999, new MappingRecord("mythical", "#8846FF") },
    { 24999, new MappingRecord("legendary", "#D22CE6") },
    { 29999, new MappingRecord("ancient", "#EB4B4B") },
    { int.MaxValue, new MappingRecord("unusual", "#FED700") },
};

while (true)
{
    Console.WriteLine("Fetch Data");
    try
    {
        await Generate();
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
    }
    Thread.Sleep(TimeSpan.FromMinutes(1));
}


async Task Generate()
{
    var options = new RestClientOptions("https://api.cs-prod.leetify.com/api/");
    var client = new RestClient(options);
    var request = new RestRequest($"profile/id/{steam64Id}");
    var response = await client.ExecuteAsync<JsonObject>(request);

    var games = response.Data!["games"]!.AsArray();
    var firstGame = (int?)null;
    var lastGame = (int?)null;
    var today = new DateTimeOffset(DateTime.Today);
    var totals = new TotalRecord();
    foreach (JsonObject game in games)
    {
        if(game == null || !game.ContainsKey("rankType") || game["rankType"]!.GetValue<int>() != 11)
            continue;

        firstGame = game["skillLevel"]!.GetValue<int>();
        lastGame ??= firstGame;
    
        var dateTime = DateTimeOffset.Parse(game["gameFinishedAt"]!.ToString()).ToLocalTime();
        if (dateTime < today)
            break;
    
        var requestGame = new RestRequest($"games/{game["gameId"]!.GetValue<string>()}");
        var responseGame = await client.ExecuteAsync<JsonObject>(requestGame);

        var playerStats = responseGame.Data!["playerStats"]!.AsArray().Cast<JsonObject>()
            .First(obj => obj["steam64Id"]!.GetValue<string>() == steam64Id)!;
        
        totals.Games++;
        totals.Kills += playerStats["totalKills"]!.GetValue<int>();
        totals.Deaths += playerStats["totalDeaths"]!.GetValue<int>();
        totals.HsSum += playerStats["hsp"]!.GetValue<double>();
        totals.AdrSum += playerStats["dpr"]!.GetValue<double>();

        var result = game["matchResult"]!.GetValue<string>();
        switch (result)
        {
            case "win":
                totals.Win++;
                break;
            case "loss":
                totals.Loss++;
                break;
            case "tie":
                totals.Draw++;
                break;
        }

    }

    if (totals.Games > 0)
    {
        totals.KdRate = (double)totals.Kills / totals.Deaths;
        totals.HsRate = (decimal)totals.HsSum / totals.Games;
        totals.WinRate = (decimal) totals.Win / (totals.Win + totals.Loss);
        totals.Adr = (double) totals.AdrSum / (totals.Games);
    }

    GenerateIcon(Path.Combine(output, "last.png"), lastGame!.Value);
    GenerateIcon(Path.Combine(output, "first.png"), firstGame!.Value);

    GenerateStats(Path.Combine(output, "cs2stats"), totals);

}


Console.WriteLine();


void GenerateStats(string path, TotalRecord totals)
{
    //File.WriteAllText(file, JsonConvert.SerializeObject(totals));
    foreach (var propertyInfo in totals.GetType().GetProperties())
    {
        var value = string.Empty;
        if (propertyInfo.PropertyType == typeof(decimal))
        {
            value = ((decimal)propertyInfo.GetValue(totals)!).ToString("P");
        }
        else if (propertyInfo.PropertyType == typeof(double))
        {
            value = ((double)propertyInfo.GetValue(totals)!).ToString("N2");
        }
        else
        {
            value = propertyInfo.GetValue(totals)!.ToString();
        }
        File.WriteAllText(Path.Combine(path, $"{propertyInfo.Name}.txt"), value);
    }
}

void GenerateIcon(string file, int rank)
{
    var mapping = FindMapping(rank);

    var bitmap = SKBitmap.Decode(Path.Combine("Assets", $"rating.{mapping.Icon}.png"));
    
    // Small Text
    using SKCanvas canvas = new(bitmap);
    canvas.Skew(new SKPoint(-.3f, 0));

    using SKPaint paint = new()
    {
        Color = SKColors.Black,
        IsAntialias = true,
    };

    var fontSize = 50f;

    var font = new SKFont
    {
        Size = fontSize*0.75f,
        Typeface = SKTypeface.FromFamilyName(
            familyName: "Roboto",
            weight: SKFontStyleWeight.Bold,
            width: SKFontStyleWidth.Normal,
            slant: SKFontStyleSlant.Upright),
    };
    
    var smallText = "," + (rank % 1000).ToString("000");
    var measureSmall = font.MeasureText(smallText, paint);

    // Big Text
    using SKCanvas canvas1 = new(bitmap);
    canvas1.Skew(new SKPoint(-.3f, 0));

    using SKPaint paint1 = new()
    {
        Color = SKColors.Black,
        IsAntialias = true,
    };

    var fontBig = new SKFont
    {
        Size = fontSize,
        Typeface = SKTypeface.FromFamilyName(
            familyName: "Roboto",
            weight: SKFontStyleWeight.Bold,
            width: SKFontStyleWidth.Normal,
            slant: SKFontStyleSlant.Upright),
    };
    var bigText = (rank / 1000).ToString();
    
    var measureBig = fontBig.MeasureText(bigText, paint1);

    var offset = 40;
    var startX = (178-offset) / 2 - (measureSmall + measureBig) / 2;
    
    // Draw small
    var coord = new SKPoint(startX + offset + measureBig, 54);
    paint.Color = new SKColor(0, 0, 0);
    canvas.DrawText(smallText, coord, font, paint);
    
    
    paint.Color = SKColor.Parse(mapping.TextColor);
    coord.Offset(0, -2);
    canvas.DrawText(smallText, coord, font, paint);
    
    //Draw big
    var coord1 = new SKPoint(startX + offset, coord.Y);
    paint1.Color = new SKColor(0, 0, 0);
    canvas1.DrawText(bigText, coord1, fontBig, paint1);
    
    paint1.Color = SKColor.Parse(mapping.TextColor);
    coord1.Offset(0, -2);
    canvas.DrawText(bigText, coord1, fontBig, paint1);
    
    using var stream = new FileStream(file, FileMode.OpenOrCreate);
    bitmap.Encode(stream, SKEncodedImageFormat.Png, 100);
    stream.Close();
}

MappingRecord FindMapping(int rank)
{
    return mapping.First(kvp => rank <= kvp.Key).Value;
}

record MappingRecord(string Icon, string TextColor);

public record TotalRecord
{
    public int Kills { get; set; }
    public int Deaths { get; set; }
    public int Games { get; set; }
    public double HsSum { get; set; }
    public double AdrSum { get; set; }
    public double KdRate { get; set; }
    public int Win { get; set; }
    public int Loss { get; set; }
    public int Draw { get; set; }
    public decimal HsRate { get; set; }
    public decimal WinRate { get; set; }
    public double Adr { get; set; }
}