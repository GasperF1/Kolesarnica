namespace MonkeyFinder.ViewModel;

[QueryProperty(nameof(Monkey), "Monkey")]
public partial class MonkeyDetailsViewModel : BaseViewModel
{
    IMap map;
    public MonkeyDetailsViewModel(IMap map)
    {
        this.map = map;
    }

    [ObservableProperty]
    MonkeyVM monkey;

    [RelayCommand]
    async Task OpenMap()
    {
        
        try
        {
            await map.OpenAsync(Monkey.trentnaLokacijaLatitude, Monkey.trentnaLokacijaLongitude, new MapLaunchOptions
            {
                Name = Monkey.znamka,
                NavigationMode = NavigationMode.None
            });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to launch maps: {ex.Message}");
            await Shell.Current.DisplayAlert("Error, no Maps app!", ex.Message, "OK");
        }
    }
}
