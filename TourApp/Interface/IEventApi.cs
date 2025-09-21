namespace TourApp.Interfaces;

public interface IEventApi
{
    Task<IReadOnlyList<TourApp.Models.TourDto>> GetToursAsync(CancellationToken ct = default);
    Task<TourApp.Models.SignupResponse> SubmitAsync(TourApp.Models.SignupFormModel model, CancellationToken ct = default);
}
