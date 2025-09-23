
namespace TourApp.Common.Models;

public class CancelBooking
{
  public required Guid TourId { get; init; }

  public required string SignupEmail { get; init; }
}
