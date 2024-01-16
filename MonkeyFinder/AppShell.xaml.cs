namespace MonkeyFinder;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        //references the details page
        //can juse put the string "DetailsPage" if not wanting to reference
        Routing.RegisterRoute(nameof(DetailsPage), typeof(DetailsPage));
    }
}