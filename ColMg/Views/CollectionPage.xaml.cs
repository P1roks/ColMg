using ColMg.ViewModels;

namespace ColMg.Views;

public partial class CollectionPage : ContentPage
{
	public CollectionPage(CollectionViewModel binding)
	{
		InitializeComponent();

		BindingContext = binding;
	}
}