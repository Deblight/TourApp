using TourApp.Blazor.Models;

namespace TourApp.Blazor.Interfaces;

public interface IEventApi
{
    Task<IReadOnlyList<TourDto>> GetToursAsync(CancellationToken ct = default);
    Task<SignupResponse> SubmitAsync(SignupFormModel model, CancellationToken ct = default);
}
