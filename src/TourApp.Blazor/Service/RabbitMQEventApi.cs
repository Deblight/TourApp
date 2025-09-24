using TourApp.Application.Interfaces;
using TourApp.Blazor.Interfaces;
using TourApp.Blazor.Models;
using TourApp.Common.Models;

namespace TourApp.Blazor.Services;

public class RabbitMQEventApi : IEventApi
{
  private readonly IBookingService _bookingService;

  private readonly List<TourDto> _tours =
  [
    new(Guid.Parse("11111111-1111-1111-1111-111111111111"), "Morning Tour"),
    new(Guid.Parse("22222222-2222-2222-2222-222222222222"), "City Highlights"),
    new(Guid.Parse("33333333-3333-3333-3333-333333333333"), "Evening Lights")
  ];

  public RabbitMQEventApi(IBookingService bookingService)
  {
    _bookingService = bookingService;
  }

  public async Task<bool> SetupAsync(CancellationToken cancellationToken=default)
  {
    return await _bookingService.SetupAsync();
  }

  public Task<IReadOnlyList<TourDto>> GetToursAsync(CancellationToken ct = default)
  {
    return Task.FromResult<IReadOnlyList<TourDto>>(_tours);
  }

  public async Task<SignupResponse> SubmitAsync(SignupFormModel model, CancellationToken ct = default)
  {
    if (model.Action is SignupAction.Book)
    {
      CreateBooking booking = new()
      {
        TourId = (Guid)model.TourId!,
        SignupEmail = model.Email,
        SignupName = model.Name
      };

      bool sent = await _bookingService.CreateBookingAsync(booking, ct);

      return new SignupResponse(true, "Signup success");
    }
    else
    {
      CancelBooking cancel = new()
      {
        TourId = (Guid)model.TourId!,
        SignupEmail = model.Email
      };

      bool sent = await _bookingService.DeleteBookingAsync(cancel, ct);

      return new SignupResponse(true, "Cancel success");
    }
  }
}
