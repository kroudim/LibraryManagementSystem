using System.Threading.Tasks;

namespace Reservation.Application.Services;

public interface IEventPublisher
{
    Task PublishAsync<T>(T @event) where T : class;
}
