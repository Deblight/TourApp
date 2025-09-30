using TourApp.Blazor.Interfaces;
using TourApp.Blazor.Models;

namespace TourApp.Blazor.Services;

public sealed class DemoEventApi : IEventApi
{
    private readonly List<TourDto> _tours =
    [
        new(Guid.Parse("11111111-1111-1111-1111-111111111111"), "Morning Tour"),
        new(Guid.Parse("22222222-2222-2222-2222-222222222222"), "City Highlights"),
        new(Guid.Parse("33333333-3333-3333-3333-333333333333"), "Evening Lights")
    ];

    private readonly HashSet<string> _bookings = new(StringComparer.OrdinalIgnoreCase);
    private readonly object _gate = new();

  public Task<bool> SetupAsync(CancellationToken cancellationToken=default)
  {
    return Task.FromResult(true);
  }

    public Task<IReadOnlyList<TourDto>> GetToursAsync(CancellationToken ct = default)
        => Task.FromResult<IReadOnlyList<TourDto>>(_tours);

    public Task<SignupResponse> SubmitAsync(SignupFormModel m, CancellationToken ct = default)
    {
        var key = $"{m.Email.Trim().ToLowerInvariant()}::{m.TourId}";
        lock (_gate)
        {
            if (m.Action == SignupAction.Book)
            {
                if (_bookings.Contains(key))
                    return Task.FromResult(new SignupResponse(true, "Allerede tilmeldt denne tour."));
                _bookings.Add(key);
                return Task.FromResult(new SignupResponse(true, "Du er nu tilmeldt. Velkommen!"));
            }
            else
            {
                if (_bookings.Remove(key))
                    return Task.FromResult(new SignupResponse(true, "Din tilmelding er annulleret."));
                return Task.FromResult(new SignupResponse(false, "Ingen aktiv tilmelding at annullere."));
            }
        }
    }
}
