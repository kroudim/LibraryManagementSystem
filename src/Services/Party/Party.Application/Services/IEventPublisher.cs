namespace Party.Application.Services;

public interface IEventPublisher
{
    Task PublishAsync<T>(T @event) where T : class;
}
