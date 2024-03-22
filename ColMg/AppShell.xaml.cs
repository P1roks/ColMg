using ColMg.Views;

namespace ColMg
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(EditElementPage), typeof(EditElementPage));
            Routing.RegisterRoute(nameof(CollectionPage), typeof(CollectionPage));
            Routing.RegisterRoute(nameof(AddColumnPage), typeof(AddColumnPage));
        }
    }
}