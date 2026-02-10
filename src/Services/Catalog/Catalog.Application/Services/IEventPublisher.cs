namespace Catalog.Application.Services;

public interface IEventPublisher
{
    Task PublishAsync<T>(T @event) where T : class;
}
