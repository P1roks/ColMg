using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColMg.ViewModels
{
    internal class EditItemViewModel : IQueryAttributable
    {
        public ObservableCollection<(string,string)> Item { get; set; } = new();

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("item"))
            {
                var enumerableItem = (IEnumerable<(string,string)>)query["item"];

                foreach(var single in enumerableItem)
                {
                    Item.Add(single);
                }
            }
        }
    }
}
