using MonkeyFinder.Services;
using MonkeyFinder;
namespace MonkeyFinder.ViewModel;

public partial class MonkeysViewModel : BaseViewModel
{
    public ObservableCollection<MonkeyVM> Monkeys { get; } = new();
    public ObservableCollection<Monkey> MonkeysGPS { get; } = new();

    MonkeyService monkeyService;
    IConnectivity connectivity;
    IGeolocation geolocation;
    public MonkeysViewModel(MonkeyService monkeyService, IConnectivity connectivity, IGeolocation geolocation)
    {
        Title = "Monkey Finder";
        this.monkeyService = monkeyService;
        this.connectivity = connectivity;
        this.geolocation = geolocation;
    }

    [ObservableProperty]
    bool isRefreshing;

    [RelayCommand]
    async Task GetMonkeysAsync()
    {
        if (IsBusy)
            return;

        try
        {
            if (connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Shell.Current.DisplayAlert("No connectivity!",
                    $"Please check internet and try again.", "OK");
                return;
            }

            IsBusy = true;
            var monkeys = await monkeyService.GetMonkeys();

            if (Monkeys.Count != 0)
            {
                Monkeys.Clear();
                MonkeysGPS.Clear();
            }

            foreach (var kolo in monkeys)
            {
                MonkeyVM zLok = new MonkeyVM();
                
                zLok.lastnik = kolo.lastnik;
                zLok.znamka = kolo.znamka;
                zLok.slika = kolo.slika;
                zLok.trentnaLokacijaLatitude = kolo.trentnaLokacijaLatitude;
                zLok.trentnaLokacijaLongitude = kolo.trentnaLokacijaLongitude;
                zLok.naslov = await GeoService.GetAddressFromCoordinates(kolo.trentnaLokacijaLatitude, kolo.trentnaLokacijaLongitude);
                Monkeys.Add(zLok);
                MonkeysGPS.Add(kolo);
            }

        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to get monkeys: {ex.Message}");
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }

    }
    
    [RelayCommand]
    async Task GoToDetails(MonkeyVM monkey)
    {
        if (monkey == null)
        return;

        await Shell.Current.GoToAsync(nameof(DetailsPage), true, new Dictionary<string, object>
        {
            {"Monkey", monkey }
        });
    }

    [RelayCommand]
    async Task GetClosestMonkey()
    {
        if (IsBusy || Monkeys.Count == 0)
            return;

        try
        {
            // Get cached location, else get real location.
            var location = await geolocation.GetLastKnownLocationAsync();
            if (location == null)
            {
                location = await geolocation.GetLocationAsync(new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.Medium,
                    Timeout = TimeSpan.FromSeconds(30)
                });
            }

            // Find closest monkey to us
            var first = MonkeysGPS.OrderBy(m => location.CalculateDistance(
                new Location(m.trentnaLokacijaLatitude, m.trentnaLokacijaLongitude), DistanceUnits.Miles))
                .FirstOrDefault();

            await Shell.Current.DisplayAlert("", first.znamka + " " +
                first.lastnik, "OK");

        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to query location: {ex.Message}");
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
    }
}
