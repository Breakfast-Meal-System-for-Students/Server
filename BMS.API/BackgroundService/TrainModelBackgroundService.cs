

using BMS.BLL.Services.IServices;

public class TrainModelBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public TrainModelBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var nextRunTime = GetNextSaturdayAtTime(00, 00);
            var delay = nextRunTime - DateTime.UtcNow;
            await Task.Delay(delay, stoppingToken);
            await TrainingModel(stoppingToken);
            //await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
        }
    }
    
    private async Task TrainingModel(CancellationToken stoppingToken)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();
            await orderService.TrainModelFromOrderHistory();
        }
    }

    private DateTime GetNextSaturdayAtTime(int hour, int minute)
    {
        var now = DateTime.UtcNow;
        var daysUntilSaturday = ((int)DayOfWeek.Saturday - (int)now.DayOfWeek + 7) % 7;
        var nextSaturday = now.AddDays(daysUntilSaturday == 0 ? 7 : daysUntilSaturday);

        return new DateTime(nextSaturday.Year, nextSaturday.Month, nextSaturday.Day, hour, minute, 0, DateTimeKind.Utc);
    }
}
