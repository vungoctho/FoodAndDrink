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

        //private readonly Lazy<Settings> _settings;
        //public Settings Settings { get { return _settings.Value; } }

        private SiteMaintenance()
        {
            _content = new Lazy<IContentService>(() => ApplicationContext.Current.Services.ContentService);
            _umbraco = new Lazy<UmbracoHelper>(() => new UmbracoHelper(UmbracoContext.Current));


            //_settings = new Lazy<Settings>(() => ContentHelper.Current.SettingsNode != null ? ContentHelper.Current.SettingsNode.As<Settings>() : new Settings());
        }

        //TODO: will call refresh if some common settings has changes
        public void Refresh()
        {
            ContentHelper.Current.Refresh();
            _instance = new Lazy<SiteMaintenance>(() => new SiteMaintenance(), true);
        }

        /// <summary>
        /// Auto generate current year / months folder
        /// </summary>
        public void GenerateFolders(params int[] years)
        {
            if (years == null) return;

            lock (_lock)
            {
                foreach (var year in years)
                {
                    LogHelper.Info(this.GetType(), $"Generate Folders {year}");
                    GenerateFolder(year);
                }
            }
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

        private void GenerateFolder(int year)
        {
            var currentYear = year.ToString();

            var nodes = ContentHelper.Current.RootNode.Children().Where("generateFolder == true");

            foreach (var node in nodes)
            {
                var folderTypeAlias = node.GetPropertyValue<string>("folderTypeAlias");

                var currentYearNode = node.Children(folderTypeAlias).FirstOrDefault(x => x.Name == currentYear);
                if (currentYearNode == null)
                {
                    var result = Content.SaveAndPublishWithStatus(Content.CreateContent(currentYear, node.Id, folderTypeAlias));
                    currentYearNode = Umbraco.TypedContent(result.Result.ContentItem.Id);
                }

                var children = currentYearNode.Children(folderTypeAlias);

                //Months
                var sortChildren = new List<IContent>();
                for (var i = 1; i <= 12; i++)
                {
                    var monthLabel = i.ToString("D2");
                    var monthNode = children.FirstOrDefault(x => x.Name == monthLabel);
                    if (monthNode == null)
                    {
                        var result = Content.SaveAndPublishWithStatus(Content.CreateContent(monthLabel, currentYearNode.Id, folderTypeAlias));
                        sortChildren.Add(result.Result.ContentItem);
                    }
                    else
                    {
                        sortChildren.Add(Content.GetById(monthNode.Id));
                    }
                }

                //Sort months
                Content.Sort(sortChildren);

                //Sort years
                var yearNodes = node.Children().Concat(new[] { currentYearNode }).OrderByDescending(x => x.Name).Select(x => x.Id);
                Content.Sort(Content.GetByIds(yearNodes));
            }
        }
    }
}
