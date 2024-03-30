using ColMg.Models;
using ColMg.Views;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Diagnostics;

namespace ColMg.ViewModels
{
    public partial class CollectionViewModel : IQueryAttributable
    {
        public string Name { get; set; } = "Collection";
        public CollectionHandler Collection { get; set; } = new();

        public event PropertyChangedEventHandler PropertyChanged;

        [RelayCommand]
        public async Task AddColumn()
        {
            string columnName = await Shell.Current.DisplayPromptAsync("Add Column", "Enter column name");
            if (columnName == null) return;

            Collection.AddColumn(columnName);
        }

        [RelayCommand]
        public async Task EditItem(CollectionItem item)
        {
            var describedItem = Collection.Columns.Zip(item.Fields);
            await Shell.Current.GoToAsync(nameof(EditIemPage),
                new Dictionary<string, object>() { {"item", describedItem}, { "idx", Collection.Items.IndexOf(item) } });
        }

        [RelayCommand]
        public async Task AddItem(CollectionItem item)
        {
            var columns = Collection.Columns.SkipLast(2);
            await Shell.Current.GoToAsync(nameof(EditIemPage),
                new Dictionary<string, object>() { {"columns", columns} });
        }

        [RelayCommand]
        public async Task DeleteItem(CollectionItem item) 
        {
            Collection.Items.Remove(item);
        }

        [RelayCommand]
        public async Task ToggleSold(CollectionItem item)
        {
            int itemIdx = Collection.Items.IndexOf(item);
            // Move to first non-sold space or to end
            int moveIdx = item.Sold ? Collection.Items.TakeWhile(i => !i.Sold).Count() : Collection.Items.Count - 1;
            item.toggleSold();
            Collection.Items.Move(itemIdx, moveIdx);
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("collection"))
            {
                // TODO: load collection with given name
                Name = (string)query["collection"];
                Trace.WriteLine(query["collection"]);
            }

            if (query.ContainsKey("item"))
            {
                CollectionItem item = (CollectionItem)query["item"];
                int? idx = (int?)query["idx"];
                if(idx is not null)
                {
                    Collection.Items[idx.Value] = item;
                }
                else
                {
                    if(Collection.Items.Any(other => other.Fields.SequenceEqual(item.Fields)))
                    {
                        if(!await Shell.Current.DisplayAlert("Item Exists",
                            "Provided item already exists. Do you want to add it?", "Yes", "No"))
                        {
                            return;
                        }
                    }
                    Collection.Items.Add(item);
                }
            }
        }
    }
}
