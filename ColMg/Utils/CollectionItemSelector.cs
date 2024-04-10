using ColMg.Models;
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
        public DataTemplate StatusTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if(item is ItemStatus)
            {
                return StatusTemplate;
            }
            else if(item is string itemString && CollectionRepository.GetImagePath(itemString) != null)
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
