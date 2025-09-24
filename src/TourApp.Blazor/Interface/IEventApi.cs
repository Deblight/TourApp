using TourApp.Blazor.Models;

namespace TourApp.Blazor.Interfaces;

public interface IEventApi
{
    Task<bool> SetupAsync(CancellationToken cancellationToken=default);
    Task<IReadOnlyList<TourDto>> GetToursAsync(CancellationToken ct = default);
    Task<SignupResponse> SubmitAsync(SignupFormModel model, CancellationToken ct = default);
}
