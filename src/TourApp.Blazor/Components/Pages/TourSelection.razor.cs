
using Microsoft.AspNetCore.Components;
using TourApp.Blazor.Interfaces;
using TourApp.Blazor.Models;

namespace TourApp.Blazor.Components.Pages;

public partial class TourSelection: ComponentBase
{
  [Inject]
  public required IEventApi Api { get; set; }

  private SignupFormModel model = new();
  private List<TourDto> tours = new();
  private bool busy;
  private SignupResponse? toast;

  protected override async Task OnInitializedAsync()
      => tours = (await Api.GetToursAsync()).ToList();

  private async Task HandleSubmit()
  {
    toast = null;
    busy = true;
    try
    {
      var res = await Api.SubmitAsync(model);
      toast = res;

      if (res.Success && model.Action == SignupAction.Book)
        model = new SignupFormModel { Action = SignupAction.Book };
    }
    finally
    {
      busy = false;
      StateHasChanged();
      await Task.Delay(2000);
      toast = null;
      StateHasChanged();
    }
  }
}
