using TourApp.Common.Models;

namespace TourApp.Application.Interfaces;

public interface IBookingService
{
  Task<bool> CreateBookingAsync(CreateBooking booking, CancellationToken cancellationToken=default);
  Task<bool> DeleteBookingAsync(CancelBooking cancellation, CancellationToken cancellationToken=default);
}
