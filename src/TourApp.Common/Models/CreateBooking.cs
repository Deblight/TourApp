
namespace TourApp.Common.Models;

public class CreateBooking
{
  public required Guid TourId { get; init; }

  public required string SignupName { get; init; }
  public required string SignupEmail { get; init; }
}
