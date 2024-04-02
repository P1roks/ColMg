using ColMg.Models;
using ColMg.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;

namespace ColMg.ViewModels
{
    public partial class CollectionViewModel : ObservableObject, IQueryAttributable
    {
        [ObservableProperty]
        public string name = "Collection";
        public CollectionHandler Collection { get; set; } = new();

        [RelayCommand]
        public async Task AddColumn()
        {
            string columnName = await Shell.Current.DisplayPromptAsync("Add Column", "Enter column name");
            if (columnName == null) return;

            Collection.AddColumn(columnName);
            CollectionRepository.saveCollection(Collection, Name);
        }

        [RelayCommand]
        public async Task EditItem(CollectionItem item)
        {
            var describedItem = Collection.Columns.Zip(item.Fields);
            await Shell.Current.GoToAsync(nameof(EditIemPage),
                new Dictionary<string, object>()
                { {"item", describedItem}, { "idx", Collection.Items.IndexOf(item) }, { "status", item.Status} });
        }

        [RelayCommand]
        public async Task AddItem(CollectionItem item)
        {
            var columns = Collection.Columns.SkipLast(2);
            await Shell.Current.GoToAsync(nameof(EditIemPage),
                new Dictionary<string, object>() { {"columns", columns} });
        }

        [RelayCommand]
        public void DeleteItem(CollectionItem item) 
        {
            Collection.Items.Remove(item);
            CollectionRepository.saveCollection(Collection, Name);
        }

        [RelayCommand]
        public async Task GenerateSummary()
        {
            string summaryText =
                $"Owned Items: {Collection.Items.Count()}\n" +
                $"Sold Items: {Collection.Items.Where(x => x.Status == ItemStatus.Sold).Count()}\n" +
                $"Items For Sale: {Collection.Items.Where(x => x.Status == ItemStatus.ForSale).Count()}";

            await Shell.Current.DisplayAlert("Summary", summaryText, "Close");
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("collection"))
            {
                Name = (string)query["collection"];
                Collection.Load(Name);
            }

            if (query.ContainsKey("item"))
            {
                CollectionItem item = (CollectionItem)query["item"];
                int? maybeIdx = (int?)query["idx"];
                if(maybeIdx is int idx)
                {
                    ItemStatus prevStatus = Collection.Items[idx].Status;
                    ItemStatus newStatus = item.Status;
                    Collection.Items[idx] = item;

                    // Group together sold items at the end
                    if(newStatus == ItemStatus.Sold && prevStatus != ItemStatus.Sold)
                    {
                        Collection.Items.Move(idx, Collection.Items.Count() - 1);
                    }
                    else if(prevStatus == ItemStatus.Sold &&  newStatus != ItemStatus.Sold)
                    {
                        Collection.Items.Move(idx, 0);
                    }
                }
                else
                {
                    // Check if this item already exists
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
                CollectionRepository.saveCollection(Collection, Name);
            }
        }
    }
}
