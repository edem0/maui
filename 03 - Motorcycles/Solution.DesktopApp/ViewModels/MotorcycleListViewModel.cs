using ErrorOr;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Solution.DesktopApp.ViewModels;

[ObservableObject]
public partial class MotorcycleListViewModel(IMotorcycleService motorcycleService)
{
    #region life cycle commands
    public IAsyncRelayCommand AppearingCommand => new AsyncRelayCommand(OnAppearingAsync);
    public IAsyncRelayCommand DisappearingCommand => new AsyncRelayCommand(OnDisappearingAsync);
    #endregion
    #region page commands
    public ICommand PreviousPageCommand { get; private set; }
    public ICommand NextPageCommand { get; private set; }

    #endregion

    public IAsyncRelayCommand DeleteCommand => new AsyncRelayCommand<string>((id) => OnDeleteAsync(id));

    [ObservableProperty]
    private ObservableCollection<MotorcycleModel> motorcycles;

    private int page = 1;
    private bool isLoading = false;
    private bool hasNextPage = false;
    private int numberOfMotorcyclesInDb = 0;

    private async Task OnAppearingAsync()
    {
        PreviousPageCommand = new Command(async () => await OnPreviousPageAsync(), () => page > 1 && !isLoading);
        NextPageCommand = new Command(async () => await OnNextPageAsync(), () => !isLoading && hasNextPage);
        
        await LoadMotorcyclesAsync();
    }

    private async Task OnDisappearingAsync()
    { }

    private async Task LoadMotorcyclesAsync()
    {
        isLoading = true;

        var result = await motorcycleService.GetPagedAsync(page);

        if (result.IsError)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Motorcycles not loaded!", "OK");
            return;
        }

        Motorcycles = new ObservableCollection<MotorcycleModel>(result.Value.Items);
        numberOfMotorcyclesInDb = result.Value.Count;

        hasNextPage = numberOfMotorcyclesInDb - (page * 10) > 0;
        isLoading = false;

        ((Command)PreviousPageCommand).ChangeCanExecute();
        ((Command)NextPageCommand).ChangeCanExecute();
    }

    private async Task OnPreviousPageAsync() 
    {
        if (isLoading) return;

        page = page <= 1 ? 1 : --page;
        await LoadMotorcyclesAsync();
    }

    private async Task OnNextPageAsync()
    {
        if (isLoading) return;
        
        page++;
        await LoadMotorcyclesAsync();
    }

    private async Task OnDeleteAsync(string? id) 
    {
        var result = await motorcycleService.DeleteAsync(id);

        var message = result.IsError ? result.FirstError.Description : "Motorcycle deleted.";
        var title = result.IsError ? "Error" : "Information";

        if (!result.IsError)
        {
            var motorcycle = motorcycles.SingleOrDefault(x => x.Id == id);
            motorcycles.Remove(motorcycle);

            if(motorcycles.Count == 0)
            {
                await LoadMotorcyclesAsync();
            }
        }

        await Application.Current.MainPage.DisplayAlert(title, message, "OK");
    }
}
