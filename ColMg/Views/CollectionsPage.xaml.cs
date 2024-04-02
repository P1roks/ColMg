using ColMg.ViewModels;

namespace ColMg.Views;

public partial class CollectionsPage : ContentPage
{
	public CollectionsPage(CollectionsViewModel binding)
	{
		InitializeComponent();

		BindingContext = binding;
	}
}