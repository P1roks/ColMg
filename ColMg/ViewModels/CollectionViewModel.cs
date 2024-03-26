using ColMg.Models;
using ColMg.Views;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;

namespace ColMg.ViewModels
{
    public partial class CollectionViewModel : IQueryAttributable
    {
        public string Name { get; set; } = "Collection";
        public CollectionHandler Items { get; set; } = new();

        [RelayCommand]
        public async Task EditItem(CollectionItem item)
        {
            var describedItem = Items.Columns.Zip(item);
            await Shell.Current.GoToAsync(nameof(EditIemPage), new Dictionary<string, object>() { {"item", describedItem} });
        }

        [RelayCommand]
        public async Task DeleteItem(CollectionItem item) 
        {
            Items.Remove(item);
        }

        [RelayCommand]
        public async Task AddItem(CollectionItem item)
        {
            // TODO
        }

        [RelayCommand]
        public async Task ToggleSold(CollectionItem item)
        {
            int itemIdx = Items.IndexOf(item);
            // Move to first non-sold space or to end
            int moveIdx = item.Sold ? Items.TakeWhile(i => !i.Sold).Count() : Items.Count - 1;
            item.toggleSold();
            Items.Move(itemIdx, moveIdx);
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("collection"))
            {
                // TODO: load collection with given name
                Name = (string)query["collection"];
                Trace.WriteLine(query["collection"]);
            }
        }
    }
}
