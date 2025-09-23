
namespace TourApp.Infrastructure.Interfaces;

public interface IBookingRepository
{
  Task<bool> PlaceBookingAsync(object booking, CancellationToken cancellationToken=default);
  Task<bool> CancelBookingAsync(object cancellation, CancellationToken cancellationToken=default);
}
