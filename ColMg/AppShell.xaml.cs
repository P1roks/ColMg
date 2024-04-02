using ColMg.Views;

namespace ColMg
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(EditIemPage), typeof(EditIemPage));
            Routing.RegisterRoute(nameof(CollectionPage), typeof(CollectionPage));
        }
    }
}