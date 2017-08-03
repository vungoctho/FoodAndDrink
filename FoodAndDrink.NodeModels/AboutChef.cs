using Our.Umbraco.Ditto;
using Umbraco.Core.Models;

namespace FoodAndDrink.NodeModels
{
    public class AboutChef : Node
    {
        [UmbracoProperty("Photo")]
        public IPublishedContent Photo { get; set; }

        public string PersonSummary { get; set; }

        public string DisplayName { get; set; }
    }
}
