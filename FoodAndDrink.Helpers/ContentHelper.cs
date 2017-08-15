using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace FoodAndDrink.Helpers
{
    //public class ContentHelper
    //{
    //    private static Lazy<ContentHelper> _instance = new Lazy<ContentHelper>(() => new ContentHelper());
    //    public static ContentHelper Current { get { return _instance.Value; } }

    //    private readonly Lazy<IPublishedContent> _rootNode;
    //    public IPublishedContent RootNode { get { return _rootNode.Value; } }

    //    private readonly Lazy<IPublishedContent> _settingsNode;
    //    public IPublishedContent SettingsNode { get { return _settingsNode.Value; } }

    //    private ContentHelper()
    //    {
    //        _rootNode = new Lazy<IPublishedContent>(() => UmbracoContext.Current.ContentCache.GetAtRoot().First());
    //        _settingsNode = new Lazy<IPublishedContent>(() => RootNode.Children("fADSiteSettings").FirstOrDefault());
    //    }

    //    public void Refresh()
    //    {
    //        _instance = new Lazy<ContentHelper>(() => new ContentHelper());
    //    }
    //}
}
