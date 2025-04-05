using SkiaSharp;

namespace Sofc.TwitchStats.Service;

public class ImageOutputService
{

    private static readonly Dictionary<int, MappingRecord> Mapping = new Dictionary<int, MappingRecord>()
    {
        { 4999, new MappingRecord("common", "#B1C3D9") },
        { 9999, new MappingRecord("uncommon", "#5E98D7") },
        { 14999, new MappingRecord("rare", "#4B69FF") },
        { 19999, new MappingRecord("mythical", "#8846FF") },
        { 24999, new MappingRecord("legendary", "#D22CE6") },
        { 29999, new MappingRecord("ancient", "#EB4B4B") },
        { int.MaxValue, new MappingRecord("unusual", "#FED700") },
    };

    public async Task WritePremierIcon(string imagePath, int rank)
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
            Size = fontSize * 0.75f,
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
        var startX = (178 - offset) / 2f - (measureSmall + measureBig) / 2f;

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

        await using var stream = new FileStream(imagePath, FileMode.OpenOrCreate);
        bitmap.Encode(stream, SKEncodedImageFormat.Png, 100);
        stream.Close();
    }


    private static MappingRecord FindMapping(int rank)
    {
        return Mapping.First(kvp => rank <= kvp.Key).Value;
    }

    private record MappingRecord(string Icon, string TextColor);
}