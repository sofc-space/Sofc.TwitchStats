using Refit;
using Sofc.TwitchStats.Configuration;
using Sofc.TwitchStats.Service;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddWindowsService(options =>
{
    options.ServiceName = "Sofcs Leetify OBS Stats";
});

builder.Services.AddHostedService<Worker>();
builder.Services.AddRefitClient<ILeetifyWebService>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://api.cs-prod.leetify.com/api"));
builder.Services.Configure<AppConfig>(builder.Configuration.GetSection("AppConfig"));
builder.Services.AddScoped<GeneratorService>();
builder.Services.AddScoped<ImageOutputService>();
builder.Services.AddScoped<ValuesOutputService>();

var host = builder.Build();
host.Run();
