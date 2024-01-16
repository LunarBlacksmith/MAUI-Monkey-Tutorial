namespace MonkeyFinder.ViewModel;

[QueryProperty("Monkey", "Monkey")]
public partial class MonkeyDetailsViewModel : BaseViewModel
{
    IMap _map;
    public MonkeyDetailsViewModel(IMap map_p)
    {
        this._map = map_p;
    }

    [ObservableProperty]
    Monkey _monkey;

    [RelayCommand]
    async Task OpenMapAsync()
    {
        try
        {
            await _map.OpenAsync(Monkey.Latitude, Monkey.Longitude,
                new MapLaunchOptions
                {
                    Name = Monkey.Name,
                    // you can set the map to open in different modes such as walking, driving, cycling, none, etc.
                    NavigationMode = NavigationMode.None
                });
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert(
                "Error",
                $"Unable to open Map: {ex.Message}",
                "OK");
        }
    }

    [RelayCommand]
    async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
