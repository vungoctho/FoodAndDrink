using Our.Umbraco.Ditto;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;

namespace FoodAndDrink.NodeModels
{
    public class AboutUsPage : Node
    {
        public string AboutFoodAndDrink { get; set; }

        [UmbracoProperty("Photo")]
        public IPublishedContent Photo { get; set; }

        [UmbracoProperty("Children")]
        public virtual IEnumerable<IPublishedContent> Children { get; set; }

        public List<AboutChef> Chefs
        {
            get
            {
                if (Children == null) return new List<AboutChef>();
                var chefs = Children.Where(s => s.DocumentTypeAlias == "fADAboutChef").As<AboutChef>();
                return chefs.ToList();
            }
        }
    }
}
