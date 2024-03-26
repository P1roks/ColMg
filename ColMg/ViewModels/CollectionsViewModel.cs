using CommunityToolkit.Mvvm.Input;
using ColMg.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ColMg.ViewModels
{
    public partial class CollectionsViewModel
    {
        public List<string> Collections { get => CollectionRepository.getCollections(); }
        
        [RelayCommand]
        private async Task SelectCollection(string collectionName)
        {
            await Shell.Current.GoToAsync(nameof(CollectionPage), new Dictionary<string, object>() { {"collection", collectionName} });
        }

        [RelayCommand]
        private async Task AddCollection()
        {
            string collectionName = await Shell.Current.DisplayPromptAsync("Add Collection", "Enter new collection name: ");
            if(!string.IsNullOrWhiteSpace(collectionName) &&
               collectionName.IndexOfAny(Path.GetInvalidFileNameChars()) < 0 &&
               !CollectionRepository.collectionExists(collectionName))
            {
                CollectionRepository.createCollection(collectionName);
                await SelectCollection(collectionName);
            }
        }
    }
}
