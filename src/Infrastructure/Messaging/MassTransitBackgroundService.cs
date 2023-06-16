using MassTransit;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Messaging;
public class MassTransitBackgroundService : BackgroundService
{
    private readonly IBusControl _bus;

    public MassTransitBackgroundService(IBusControl bus) => _bus = bus;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _bus.StartAsync(stoppingToken).ConfigureAwait(false);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await _bus.StopAsync(cancellationToken);
    }
}
