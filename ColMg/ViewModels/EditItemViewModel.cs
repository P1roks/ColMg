using ColMg.Models;
using ColMg.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace ColMg.ViewModels
{
    internal partial class EditItemViewModel : ObservableObject, IQueryAttributable
    {
        public ObservableCollection<FieldDescription> Item { get; set; } = new();

        [ObservableProperty]
        public ItemStatus status;

        public List<string> Statuses
        {
            get => Enum.GetNames(typeof(ItemStatus)).Select(x => x.SplitCamelCase()).ToList();
        }

        private int? itemIdx = null;
        private string imagePath = null;

        [RelayCommand]
        public async Task SelectImage()
        {
            try
            {
                var image = await FilePicker.PickAsync(new() { FileTypes = FilePickerFileType.Images }); 
                if(image != null)
                {
                    if(image.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
                       image.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase) || 
                       image.FileName.EndsWith("gif", StringComparison.OrdinalIgnoreCase))
                    {
                        imagePath = CollectionRepository.importImage(image.FullPath);
                    }
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine($"Error: {e}");
                await Shell.Current.DisplayAlert("Pick Image", "Unexpected error has occured!", "cancel");
                return;
            }
        }

        [RelayCommand]
        public async Task SaveItem()
        {
            CollectionItem itemResponse = new(Item.Select(x => x.Value).ToList());
            itemResponse.Status = Status;
            itemResponse.ImageLocation = imagePath;
            await Shell.Current.GoToAsync("..",
                new Dictionary<string, object>() { { "item", itemResponse }, { "idx", itemIdx } });
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("item"))
            {
                if (query.ContainsKey("idx")) { itemIdx = (int?)query["idx"]; } else return;
                if (query.ContainsKey("status")) { Status = (ItemStatus)query["status"]; }

                var enumerableItem = (IEnumerable<(string, string)>)query["item"];

                foreach (var single in enumerableItem)
                {
                    Item.Add(new(single.Item1, single.Item2));
                }
            }

            if (query.ContainsKey("columns"))
            {
                var enumerableItem = (IEnumerable<string>)query["columns"];

                foreach (var single in enumerableItem)
                {
                    Item.Add(new(single, ""));
                }
            }
        }
    }
}
