using Our.Umbraco.Ditto;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;

namespace FoodAndDrink.NodeModels
{
    public class HomePage : Node
    {
        [UmbracoProperty("FoodImages")]
        public List<IPublishedContent> FoodImages { get; set; }

        public string AboutFAD { get; set; }

        [UmbracoProperty("Children")]
        public virtual IEnumerable<IPublishedContent> Children { get; set; }

        public List<HomeHighlightFood> HighlightFoods
        {
            get
            {
                if (Children == null) return new List<HomeHighlightFood>();
                var foods = Children.Where(s => s.DocumentTypeAlias == "fADHomeHighlightFood").As<HomeHighlightFood>();
                return foods.ToList();
            }
        }
    }
}
