using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColMg.ViewModels
{
    public class CollectionViewModel : IQueryAttributable
    {
        private readonly CollectionRepository repo;

        public CollectionViewModel(CollectionRepository repo)
        {
            this.repo = repo;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("collection"))
            {
                Trace.WriteLine(query["collection"]);
            }
        }
    }
}
