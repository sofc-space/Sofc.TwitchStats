using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using Refit;
using Sofc.TwitchStats.Api.Data.Api;
using Sofc.TwitchStats.Api.Data.Configuration;
using Sofc.TwitchStats.Api.Data.Leetify.V2Api.Profile;
using Sofc.TwitchStats.Api.Service;
using StackExchange.Redis;

var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy  =>
        {
            policy.WithOrigins("*");
        });
});

var jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web)
{
    Converters =
    {
        new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
    },
    NumberHandling = JsonNumberHandling.AllowReadingFromString
};

var refitSettings = new RefitSettings
{
    ContentSerializer = new SystemTextJsonContentSerializer(jsonSerializerOptions),
};

builder.Services.AddSingleton<JsonConfiguration>(s => new JsonConfiguration(jsonSerializerOptions));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddRefitClient<ILeetifyWebService>(refitSettings)
    .ConfigureHttpClient(c =>
    {
        c.BaseAddress = new Uri("https://api.cs-prod.leetify.com/api");
        c.Timeout = TimeSpan.FromSeconds(10);
    });

builder.Services.AddRefitClient<ILeetifyV2WebService>(refitSettings)
    .ConfigureHttpClient(c =>
    {
        c.BaseAddress = new Uri("https://api-public.cs-prod.leetify.com/v2");
        c.Timeout = TimeSpan.FromSeconds(10);
    });

builder.Services.AddScoped<GeneratorService>();
builder.Services.AddScoped<LeetifyCacheService>();
builder.Services.AddScoped<ResultCacheService>();
builder.Services.AddScoped<CacheService>();
builder.Services.AddScoped<IConnectionMultiplexer>(s => ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")!));
builder.Services.Configure<MetaOptions>(builder.Configuration.GetSection("Meta"));

builder.Services.AddAutoMapper(typeof(LeetifyToResultMapperProfile));

var app = builder.Build();

//using var scope = app.Services.CreateScope();
//var v2Web = scope.ServiceProvider.GetService<ILeetifyV2WebService>();
//var res = await v2Web.GetMatch(Guid.Parse("5a9d0f57-ebd9-4f24-a8fa-3fe591f5f717"));


app.UseCors(MyAllowSpecificOrigins);

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();


public class HttpLoggingHandler(ILogger<HttpLoggingHandler> logger) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Guid id = Guid.NewGuid();
        HttpResponseMessage response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Id}] Request: {Request}", id, request);
        logger.LogInformation("[{Id}] Response: {Response}", id, response);
        return response;
    }
}

public class LeetifyToResultMapperProfile : Profile
{
    public LeetifyToResultMapperProfile()
    {
        CreateMap<LeetifyV2Ranks, LeetifyRanksStatsResult>();
        CreateMap<LeetifyV2Stats, LeetifyStatsStatsResult>();
        CreateMap<LeetifyV2Rating, LeetifyRatingStatsResult>();
    }
}
