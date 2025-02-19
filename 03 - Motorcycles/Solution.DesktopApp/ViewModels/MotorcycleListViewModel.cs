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
        
        await LoadMotorcycles();
    }

    private async Task OnDisappearingAsync()
    { }

    private async Task LoadMotorcycles()
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
        await LoadMotorcycles();
    }

    private async Task OnNextPageAsync()
    {
        if (isLoading) return;
        
        page++;
        await LoadMotorcycles();
    }
}
