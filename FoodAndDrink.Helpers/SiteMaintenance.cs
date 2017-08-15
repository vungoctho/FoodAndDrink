using FoodAndDrink.NodeModels;
using Our.Umbraco.Ditto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace FoodAndDrink.Helpers
{
    public class SiteMaintenance
    {
        private static object _lock = new object();

        private static Lazy<SiteMaintenance> _instance = new Lazy<SiteMaintenance>(() => new SiteMaintenance(), true);
        public static SiteMaintenance Current { get { return _instance.Value; } }

        private Lazy<IContentService> _content;
        private IContentService Content { get { return _content.Value; } }

        private Lazy<UmbracoHelper> _umbraco;
        private UmbracoHelper Umbraco { get { return _umbraco.Value; } }

        public static IPublishedContent RootNode = UmbracoContext.Current.ContentCache.GetAtRoot().First();

        public static IPublishedContent SettingsNode = RootNode.Children("fADSiteSettings").FirstOrDefault();

        private SiteMaintenance()
        {
            _content = new Lazy<IContentService>(() => ApplicationContext.Current.Services.ContentService);
            _umbraco = new Lazy<UmbracoHelper>(() => new UmbracoHelper(UmbracoContext.Current));
        }

        //TODO: will call refresh if some common settings has changes
        public void Refresh()
        {            
            _instance = new Lazy<SiteMaintenance>(() => new SiteMaintenance(), true);
            RootNode = UmbracoContext.Current.ContentCache.GetAtRoot().First();
            SettingsNode = RootNode.Children("fADSiteSettings").FirstOrDefault();
        }

        public IPublishedContent GenerateFolder(IPublishedContent parentNode, int year, int month, int day)
        {
            bool sortYear = false;
            bool sortMonth = false;
            bool sortDay = false;
            var isAllowGenerateFolder = parentNode.GetPropertyValue<bool>("generateFolder");
            if (!isAllowGenerateFolder)
            {
                throw new Exception("This node is not allow to generate folder.");
            }
            var folderTypeAlias = parentNode.GetPropertyValue<string>("folderTypeAlias");
            var currentYearNode = parentNode.Children(folderTypeAlias).FirstOrDefault(x => x.Name == year.ToString());
            if (currentYearNode == null)
            {
                var result = Content.SaveAndPublishWithStatus(Content.CreateContent(year.ToString(), parentNode.Id, folderTypeAlias));
                currentYearNode = Umbraco.TypedContent(result.Result.ContentItem.Id);
                sortYear = true;
            }
            var monthChildren = currentYearNode.Children(folderTypeAlias);
            var monthLabel = month.ToString("D2");
            var monthNode = monthChildren.FirstOrDefault(x => x.Name == monthLabel);
            if (monthNode == null)
            {
                var result = Content.SaveAndPublishWithStatus(Content.CreateContent(monthLabel, currentYearNode.Id, folderTypeAlias));
                monthNode = Umbraco.TypedContent(result.Result.ContentItem.Id);
                sortMonth = true;
            }

            var dayChildren = monthNode.Children(folderTypeAlias);
            var dayLabel = day.ToString("D2");
            var dayNode = dayChildren.FirstOrDefault(x => x.Name == dayLabel);
            if (dayNode == null)
            {
                var result = Content.SaveAndPublishWithStatus(Content.CreateContent(dayLabel, monthNode.Id, folderTypeAlias));
                dayNode = Umbraco.TypedContent(result.Result.ContentItem.Id);
                sortDay = true;
            }

            if (sortDay)
            {
                var dayNodes = dayChildren.Concat(new[] { dayNode }).OrderBy(x => x.Name).Select(s => s.Id);
                Content.Sort(Content.GetByIds(dayNodes));
            }
            if (sortMonth)
            {
                var monthNodes = monthChildren.Concat(new[] { monthNode }).OrderBy(x => x.Name).Select(s => s.Id);
                Content.Sort(Content.GetByIds(monthNodes));
            }
            if (sortYear)
            {
                var yearNodes = parentNode.Children().Concat(new[] { currentYearNode }).OrderByDescending(x => x.Name).Select(x => x.Id);
                Content.Sort(Content.GetByIds(yearNodes));
            }

            return dayNode;
        }
        
    }
}
