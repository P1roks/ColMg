using ColMg.Models;
using ColMg.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ColMg.ViewModels
{
    public partial class CollectionViewModel : ObservableObject, IQueryAttributable
    {
        [ObservableProperty]
        public string name = "Collection";

        public CollectionHandler Collection { get; set; } = new();

        [RelayCommand]
        private async Task ExportCollection()
        {
            if (CollectionRepository.exportCollection(Name))
            {
                await Shell.Current.DisplayAlert("Export", "Exported succesfully to documents folder!", "Close");
            }
            else
            {
                await Shell.Current.DisplayAlert("Export", "Unexpected error has occured during export! Please try again", "Close");
            }
        }

        [RelayCommand]
        private async Task AddColumn()
        {
            string columnName = await Shell.Current.DisplayPromptAsync("Add Column", "Enter column name");
            if (columnName == null) return;

            Collection.AddColumn(columnName);
            CollectionRepository.saveCollection(Collection, Name);
        }

        [RelayCommand]
        private async Task EditItem(CollectionItem item)
        {
            //Skip special columns ( i.e. image, status, actions )
            var describedItem = Collection.Columns.Skip(1).SkipLast(2).Zip(item.ExtraFields);
            await Shell.Current.GoToAsync(nameof(EditIemPage),
                new Dictionary<string, object>()
                { {"item", describedItem}, { "idx", Collection.Items.IndexOf(item) }, { "status", item.Status} });
        }

        [RelayCommand]
        private async Task AddItem(CollectionItem item)
        {
            //Skip special columns ( i.e. image, status, actions )
            var columns = Collection.Columns.Skip(1).SkipLast(2);
            await Shell.Current.GoToAsync(nameof(EditIemPage),
                new Dictionary<string, object>() { { "columns", columns } });
        }

        [RelayCommand]
        private void DeleteItem(CollectionItem item)
        {
            Collection.Items.Remove(item);
            CollectionRepository.saveCollection(Collection, Name);
        }

        [RelayCommand]
        private async Task GenerateSummary()
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
                if (maybeIdx is int idx)
                {
                    ItemStatus prevStatus = Collection.Items[idx].Status;
                    ItemStatus newStatus = item.Status;
                    Collection.Items[idx] = item;

                    // Group together sold items at the end
                    if (newStatus == ItemStatus.Sold && prevStatus != ItemStatus.Sold)
                    {
                        Collection.Items.Move(idx, Collection.Items.Count() - 1);
                    }
                    else if (prevStatus == ItemStatus.Sold && newStatus != ItemStatus.Sold)
                    {
                        Collection.Items.Move(idx, 0);
                    }
                }
                else
                {
                    // Check if this item already exists
                    if (Collection.Items.Any(other => other.ExtraFields.SequenceEqual(item.ExtraFields)))
                    {
                        if (!await Shell.Current.DisplayAlert("Item Exists",
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
