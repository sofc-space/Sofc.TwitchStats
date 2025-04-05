using Sofc.TwitchStats.Data.Output;

namespace Sofc.TwitchStats.Service;

public class ValuesOutputService
{
    public async Task WriteTotalStats(string path, TotalRecord total)
    {
        foreach (var propertyInfo in total.GetType().GetProperties())
        {
            var value = string.Empty;
            if (propertyInfo.PropertyType == typeof(decimal))
            {
                value = ((decimal)propertyInfo.GetValue(total)!).ToString("P");
            }
            else if (propertyInfo.PropertyType == typeof(double))
            {
                value = ((double)propertyInfo.GetValue(total)!).ToString("N2");
            }
            else
            {
                value = propertyInfo.GetValue(total)!.ToString();
            }
            await File.WriteAllTextAsync(Path.Combine(path, $"{propertyInfo.Name}.txt"), value);
        }
    }
}