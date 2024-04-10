using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ColMg.Models
{
    public partial class CollectionHandler : ObservableObject, INotifyPropertyChanged
    {
        [ObservableProperty]
        private ObservableCollection<string> columns = new();

        [ObservableProperty]
        private ObservableCollection<CollectionItem> items = new();

        public int Width { get => Columns.Count != 0 ? Columns.Count * 100 : 500; }

        public void AddColumn(string columnName)
        {
            // Insert before options and sold
            Columns.Insert(Columns.Count - 2, columnName);
            for (int i = 0; i < Items.Count; i++)
            {
                Items[i].ExtraFields.Add("");
                //This triggers the property changed event, don't know any other way to do it
                Items[i] = Items[i];
            }
            OnPropertyChanged(nameof(Width));
        }

        public void Load(string collectionName)
        {
            (var columnsFile, var itemsFile) = CollectionRepository.GetCollection(collectionName);
            if (columnsFile is null || itemsFile is null)
            {
                return;
            }

            Columns.Clear();
            Items.Clear();

            foreach (var col in columnsFile)
            {
                Columns.Add(col);
            }

            foreach (var itm in itemsFile)
            {
                Items.Add(itm);
            }
            OnPropertyChanged(nameof(Width));
        }

        public CollectionHandler() { }
    }
}
