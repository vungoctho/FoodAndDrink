using Our.Umbraco.Ditto;
using Umbraco.Core.Models;

namespace FoodAndDrink.NodeModels
{
    public class Site : Node
    {
        public string SiteTitle { get; set; }

        public string SiteDescription { get; set; }
        
        public IPublishedContent SiteLogo { get; set; }

        //public string SiteLogoUrl
        //{
        //    get { return Umbraco.Media(SiteLogoId).Url; }
        //}

        //[UmbracoProperty("SiteLogoFooter")]
        //public string SiteLogoFooterId { get; set; }

        //public string SiteLogoFooterUrl
        //{
        //    get { return Umbraco.Media(SiteLogoFooterId).Url; }
        //}
    }
}
