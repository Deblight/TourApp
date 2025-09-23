
using Microsoft.AspNetCore.Components;

namespace TourApp.Blazor.Components.Pages;

public partial class Index: ComponentBase
{
  [Inject]
  public required NavigationManager Nav { get; set; }

  protected override void OnInitialized() => Nav.NavigateTo("/tour");
}
