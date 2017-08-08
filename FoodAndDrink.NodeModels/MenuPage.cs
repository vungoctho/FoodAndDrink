using Our.Umbraco.Ditto;
using System.Collections.Generic;
using Umbraco.Core.Models;

namespace FoodAndDrink.NodeModels
{
    public class MenuPage : Node
    {
        public string Title { get; set; }

        public string Description { get; set; }

        [UmbracoProperty("Children")]
        public virtual IEnumerable<IPublishedContent> Children { get; set; }

        [MenuFoodProcessor]
        public List<MenuFood> Foods { get; set; }
    }
}
