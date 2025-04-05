using Microsoft.Extensions.Options;
using Sofc.TwitchStats.Configuration;
using Sofc.TwitchStats.Service;

public class Worker(
    IOptions<AppConfig> options,
    GeneratorService generatorService,
    ImageOutputService imageOutputService,
    ValuesOutputService valuesOutputService,
    ILogger<Worker> logger) : BackgroundService
{

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var config = options.Value;
                foreach (var entry in config.Entries)
                {
                    try
                    {
                        logger.LogInformation($"Start generating Stats for Steam64Id {entry.Steam64Id}");
                        var total = await generatorService.GenerateStats(entry);

                        var output = EnsureDirectoryExists(entry.OutputPath);
                        
                        await imageOutputService.WritePremierIcon(Path.Combine(output, "premier-current.png"), total.PremierCurrent!.Value);
                        await imageOutputService.WritePremierIcon(Path.Combine(output, "premier-start.png"), total.PremierStart!.Value);
                        
                        var valuesPath = EnsureDirectoryExists(Path.Combine(output, "values"));
                        
                        await valuesOutputService.WriteTotalStats(valuesPath, total);
                        logger.LogInformation($"Finished generating Stats for Steam64Id {entry.Steam64Id}");
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e.Message);
                    }
                }
                logger.LogInformation($"zZzzzz");
                await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
        }
    }

    private static string EnsureDirectoryExists(string directory)
    {
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory!);
        return directory;
    }
}