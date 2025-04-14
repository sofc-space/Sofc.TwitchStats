using Refit;
using Sofc.TwitchStats.Api.Data.Configuration;
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

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddRefitClient<ILeetifyWebService>()
    .ConfigureHttpClient(c =>
    {
        c.BaseAddress = new Uri("https://api.cs-prod.leetify.com/api");
        c.Timeout = TimeSpan.FromSeconds(10);
    });
builder.Services.AddRefitClient<ILeetifyV2WebService>()
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
