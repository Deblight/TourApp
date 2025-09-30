using TourApp.Application.Interfaces;
using TourApp.Infrastructure.Interfaces;
using TourApp.Common.Models;

namespace TourApp.Application.Service;

internal class BookingService : IBookingService
{
  private readonly IBookingRepository _repository;

  public BookingService(IBookingRepository repository)
  {
    _repository = repository;
  }

  public async Task<bool> SetupAsync(CancellationToken cancellationToken=default)
  {
    return await _repository.SetupRepositoryAsync();
  }

  public async Task<bool> CreateBookingAsync(CreateBooking booking, CancellationToken cancellationToken = default)
  {
    var result = await _repository.PlaceBookingAsync(booking, cancellationToken);
    return result;
  }

  public async Task<bool> DeleteBookingAsync(CancelBooking cancellation, CancellationToken cancellationToken = default)
  {
    var result = await _repository.CancelBookingAsync(cancellation, cancellationToken);
    return result;
  }
}
