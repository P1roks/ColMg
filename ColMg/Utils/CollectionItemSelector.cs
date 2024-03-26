using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColMg.Utils
{
    public class CollectionItemSelector : DataTemplateSelector
    {
        public DataTemplate StringTemplate { get; set; }
        public DataTemplate ImageTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (Uri.IsWellFormedUriString((string)item, UriKind.Absolute))
            {
                return ImageTemplate;
            }
            else
            {
                return StringTemplate;
            }
        }
    }
}
