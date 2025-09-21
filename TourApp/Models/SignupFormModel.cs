using System.ComponentModel.DataAnnotations;

namespace TourApp.Models;

public class SignupFormModel
{
    [Required(ErrorMessage = "Navn er påkrævet")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email er påkrævet")]
    [EmailAddress(ErrorMessage = "Ugyldig email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vælg en tour")]
    public Guid? TourId { get; set; }

    [Required(ErrorMessage = "Vælg Book eller Cancel")]
    public SignupAction? Action { get; set; }
}
