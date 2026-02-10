using Audit.Application.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Audit.Infrastructure.Services;

public class DataRetentionService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DataRetentionService> _logger;

    public DataRetentionService(IServiceProvider serviceProvider, ILogger<DataRetentionService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Data retention service running at: {time}", DateTimeOffset.Now);
                
                using (var scope = _serviceProvider.CreateScope())
                {
                    var auditService = scope.ServiceProvider.GetRequiredService<AuditEventService>();
                    var oneYearAgo = DateTime.UtcNow.AddYears(-1);
                    await auditService.DeleteOldEventsAsync(oneYearAgo);
                    _logger.LogInformation("Deleted events older than {date}", oneYearAgo);
                }
                
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in data retention service");
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}
