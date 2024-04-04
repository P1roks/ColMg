﻿using ColMg.Views;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace ColMg.ViewModels
{
    public partial class CollectionsViewModel
    {
        public ObservableCollection<string> Collections { get; private set; } = new(CollectionRepository.getCollections());

        [RelayCommand]
        private async Task SelectCollection(string collectionName)
        {
            await Shell.Current.GoToAsync(nameof(CollectionPage), new Dictionary<string, object>() { { "collection", collectionName } });
        }

        [RelayCommand]
        private async Task ImportCollection()
        {
            try
            {
                var file = await FilePicker.PickAsync();
                if (file != null)
                {
                    if (CollectionRepository.importCollection(file.FullPath))
                    {
                        Collections.Add(Path.GetFileNameWithoutExtension(file.FullPath));
                    }
                    await Shell.Current.DisplayAlert("Import", "Import successful!", "Close");
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine($"Error: {e}");
                await Shell.Current.DisplayAlert("Import", "Unexpected error has occured!", "Close");
                return;
            }
        }

        [RelayCommand]
        private async Task AddCollection()
        {
            string collectionName = await Shell.Current.DisplayPromptAsync("Add Collection", "Enter new collection name: ");
            if (!string.IsNullOrWhiteSpace(collectionName) &&
               collectionName.IndexOfAny(Path.GetInvalidFileNameChars()) < 0 &&
               !CollectionRepository.collectionExists(collectionName))
            {
                CollectionRepository.createCollection(collectionName);
                Collections.Add(collectionName);
                await SelectCollection(collectionName);
            }
        }
    }
}
