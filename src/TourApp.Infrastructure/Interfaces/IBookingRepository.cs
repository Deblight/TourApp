using TourApp.Common.Models;

namespace TourApp.Infrastructure.Interfaces;

public interface IBookingRepository
{
  Task<bool> PlaceBookingAsync(CreateBooking booking, CancellationToken cancellationToken=default);
  Task<bool> CancelBookingAsync(CancelBooking cancellation, CancellationToken cancellationToken=default);
}
