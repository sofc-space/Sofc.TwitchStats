using Refit;
using Sofc.TwitchStats.Api;
using Sofc.TwitchStats.Api.Data.Api;
using Sofc.TwitchStats.Api.Service;

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
builder.Services.AddScoped<GeneratorService>();
builder.Services.AddSingleton<LeetifyCacheService>();

var app = builder.Build();

app.UseCors(MyAllowSpecificOrigins);

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
