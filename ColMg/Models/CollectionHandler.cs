using ColMg.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ColMg.Models
{
    public enum ItemStatus
    {
        New,
        Used,
        ForSale,
        Sold,
        LookingToBuy,
    }

    public class CollectionItem
    {
        public List<string> Fields { get; set; }
        public ItemStatus Status { get; set; }
        public string StatusString {
            get => Status.ToString().SplitCamelCase();
        }

        public CollectionItem(List<string> fields)
        {
            Fields = fields;
        }

        public CollectionItem() {}

        public static CollectionItem FromLine(string line)
        {
            var split = line.Split(' ');
            return new CollectionItem()
            {
                Fields = split.SkipLast(1).ToList(),
                Status = (ItemStatus)Enum.ToObject(typeof(ItemStatus), Convert.ToInt32(split.Last()))
            };
        }

        public string ToLine()
        {
            string line = string.Join(' ', Fields);
            line += $" {(int)Status}";
            return line;
        }
    }

    public partial class CollectionHandler: ObservableObject, INotifyPropertyChanged
    {
        [ObservableProperty]
        private ObservableCollection<string> columns = new();

        [ObservableProperty]
        private ObservableCollection<CollectionItem> items = new();

        public int Width { get => Columns.Count != 0 ? Columns.Count * 100 : 500; }

        public void AddColumn(string columnName)
        {
            Columns.Add(columnName);
            // Move before options and sold
            Columns.Move(Columns.Count - 1, Columns.Count - 3);
            for(int i = 0; i < Items.Count; i++)
            {
                Items[i].Fields.Add("");
                //This triggers the property changed event, probably need to change this later
                Items[i] = Items[i];
            }
            OnPropertyChanged(nameof(Width));
        }

        public void Load(string collectionName)
        {
            (var columnsFile, var itemsFile) = CollectionRepository.getCollection(collectionName);
            if(columnsFile == null || itemsFile == null)
            {
                return;
            }

            Columns.Clear();
            Items.Clear();

            foreach(var col in columnsFile)
            {
                Columns.Add(col);
            }

            foreach(var itm in itemsFile)
            {
                Items.Add(itm);
            }
            OnPropertyChanged(nameof(Width));
        }

        public CollectionHandler() { }
    }
}
