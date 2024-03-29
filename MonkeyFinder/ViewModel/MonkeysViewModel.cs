﻿using IntelliJ.Lang.Annotations;
using MonkeyFinder.Services;
using static Android.Icu.Text.CaseMap;
namespace MonkeyFinder.ViewModel;

public partial class MonkeysViewModel : BaseViewModel
{
    MonkeyService _monkeyService;
    public ObservableCollection<Monkey> Monkeys { get; } = new();

    // for internet and other connectivity checking using the service created in MauiProgram.cs
    IConnectivity _connectivity;
    IGeolocation _geolocation;
    public MonkeysViewModel(MonkeyService monkeyService_p, IConnectivity connectivity_p, IGeolocation geolocation_p)
    {
        Title = "Monkey Finder";
        this._monkeyService = monkeyService_p;
        this._connectivity = connectivity_p;
        this._geolocation = geolocation_p;
    }

    [ObservableProperty]
    bool _isRefreshing;

    [RelayCommand]
    async Task GetClosestMonkeyAsync()
    {
        if (IsBusy || Monkeys.Count <= 0)
        { return; }

        try
        {
            Location location = await _geolocation.GetLastKnownLocationAsync();
            if (location is null)
            {
                location = await _geolocation.GetLocationAsync(
                    new GeolocationRequest
                    {
                        // medium accuracy is a good balance to use if you don't need location to be super specific
                        DesiredAccuracy = GeolocationAccuracy.Medium,
                        // this will throw an error if it reaches the timeout value
                        Timeout = TimeSpan.FromSeconds(30),
                    });
            }

            if (location is null)
            { return; }

            Monkey first = Monkeys.OrderBy(m =>
            // calculating how far the closest monkey is from me
            location.CalculateDistance(m.Latitude, m.Longitude, DistanceUnits.Kilometers)
            // finding the first one
            ).FirstOrDefault();

            // if there are no monkeys returned then do nothing and break out the try/catch
            if (first is null)
            { return; }

            await Shell.Current.DisplayAlert(
                "Closest Monkey",
                $"{first.Name} in {first.Location}",
                "OK");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert(
                "Error!",
                $"Unable to get closest monkey: {ex.Message}",
                "OK");
        }
    }

    [RelayCommand]
    async Task GoToDetailsAsync(Monkey monkey_p)
    {
        if (monkey_p is null)
        { return; }

        await Shell.Current.GoToAsync(
            $"{nameof(DetailsPage)}",
            true,
            new Dictionary<string, object>
            {
                {"Monkey", monkey_p }
            });
    }

    // RelayCommand allows you to access this method outside of the class (since it's private)
    // through an autogenerated Command, using CommunityToolkit.Mvvm.Input.
    [RelayCommand]
    async Task GetMonkeyAsync()
    {
        if (IsBusy)
        { return; }

        try
        {
            // if we don't have internet
            if (_connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                // display an alert
                await Shell.Current.DisplayAlert(
                    "Warning!",
                    "Unable to connect to the internet.\nPlease check your internet connection and try again.",
                    "OK");
                // then do nothing and escape the try/catch
                return;
            }

            IsBusy = true;
            List<Monkey> monkeysList = await _monkeyService.GetMonkeys();

            if (Monkeys.Count != 0)
            { Monkeys.Clear(); }

            foreach (Monkey m in monkeysList)
            { Monkeys.Add(m); }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            // this DisplayAlert method should be abstracted using an interface -- bad practise
            await Shell.Current.DisplayAlert(
                "Error!",
                $"Unable to get monkeys: {ex.Message}", //DO NOT DISPLAY THE ERROR MESSAGE -- only done for the workshop
                "OK");
            throw;
        }
        finally //<-- Code that is executed no matter if the code exits the Try or Catch block
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }
}
