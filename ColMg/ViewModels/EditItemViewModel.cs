using ColMg.Models;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColMg.ViewModels
{
    internal partial class EditItemViewModel : IQueryAttributable
    {
        public ObservableCollection<FieldDescription> Item { get; set; } = new();
        private int? itemIdx = null;

        [RelayCommand]
        public async Task SaveItem()
        {
            CollectionItem itemResponse = new(Item.Select(x => x.Value).ToList());
            await Shell.Current.GoToAsync("..",
                new Dictionary<string, object>() { {"item",  itemResponse}, {"idx", itemIdx } });
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("item"))
            {
                if (query.ContainsKey("idx")) { itemIdx = (int?)query["idx"]; } else return;

                var enumerableItem = (IEnumerable<(string,string)>)query["item"];

                foreach(var single in enumerableItem)
                {
                    Item.Add(new(single.Item1, single.Item2));
                }
            }

            if (query.ContainsKey("columns"))
            {
                var enumerableItem = (IEnumerable<string>)query["columns"];

                foreach(var single in enumerableItem)
                {
                    Item.Add(new(single, ""));
                }
            }
        }
    }
}
