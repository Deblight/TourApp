using System.Net.Http.Json;
using TourApp.Blazor.Interfaces;
using TourApp.Blazor.Models;

namespace TourApp.Blazor.Services;

public sealed class HttpEventApi(HttpClient http) : IEventApi
{
    public async Task<IReadOnlyList<TourDto>> GetToursAsync(CancellationToken ct = default)
        => await http.GetFromJsonAsync<IReadOnlyList<TourDto>>("api/tours", ct) ?? Array.Empty<TourDto>();

    public async Task<SignupResponse> SubmitAsync(SignupFormModel model, CancellationToken ct = default)
    {
        using var res = await http.PostAsJsonAsync("api/signups", model, ct);
        res.EnsureSuccessStatusCode();
        var body = await res.Content.ReadFromJsonAsync<SignupResponse>(cancellationToken: ct);
        return body ?? new SignupResponse(false, "Uventet svar fra serveren.");
    }
}
