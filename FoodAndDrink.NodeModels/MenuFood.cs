using Our.Umbraco.Ditto;
using System;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace FoodAndDrink.NodeModels
{
    public class MenuFood : Node
    {
        public string DisplayName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        [UmbracoProperty("Image")]
        public IPublishedContent Image { get; set; }
        public string Description { get; set; }

        public decimal Price { get; set; }
    }

    public class MenuFoodProcessorAttribute : DittoProcessorAttribute
    {
        public override object ProcessValue()
        {
            var content = Value as IPublishedContent;
            if (content == null) return null;
            return content.Descendants("fADMenuFood")
                .OrderBy(s => s.SortOrder)
                .Select(s => s.As<MenuFood>())
                .ToList();
        }
    }
}
