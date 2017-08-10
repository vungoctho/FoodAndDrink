using Our.Umbraco.Ditto;
using System;
using System.Linq;
using Umbraco.Core.Models;

namespace FoodAndDrink.NodeModels
{
    public class FoodOrderDetailEntry : Node
    {
        public string FoodId { get; set; }

        public string FoodName { get; set; }

        public string FoodDescription { get; set; }

        public decimal Price { get; set; }

        public int Amount { get; set; }        
    }

    public class FoodOrderDetailEntryProcessorAttribute : DittoProcessorAttribute
    {
        public override object ProcessValue()
        {
            var content = Value as IPublishedContent;
            if (content == null) return null;
            return content.Children.Where(x => x.DocumentTypeAlias == "fADOrderDetailEntry")
                .OrderBy(s => s.SortOrder)
                .Select(s => s.As<FoodOrderDetailEntry>())
                .ToList();
        }
    }
}
