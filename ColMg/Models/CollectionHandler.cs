using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ColMg.Models
{
    public class CollectionItem: List<string>
    {
        public bool Sold { get; private set; }

        public void toggleSold()
        {
            Sold = !Sold;
        }
    }

    public partial class CollectionHandler: ObservableCollection<CollectionItem>
    {
        public List<string> Columns { get; set; } = new();
        public int Width { get => Columns.Count * 100; }

        public CollectionHandler()
        {
            Columns.AddRange(new string[] {"Nazwa", "Cena", "Opis", "Sprzedane", "Opcje"});
            Add(new() { "Pokemon", null, "cool"});
            Add(new() { "https://i.imgur.com/wCDzMBa.jpeg", "10", "cooler"});
            Add(new() { "Pokemon FireRed", "20", "[cool]"});
        }
    }
}
