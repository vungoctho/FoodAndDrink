using System;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace FoodAndDrink.NodeModels
{
    public abstract class Node
    {
        protected readonly UmbracoHelper Umbraco = new UmbracoHelper(UmbracoContext.Current);

        public DateTime CreateDate { get; set; }
        public int CreatorId { get; set; }
        public string CreatorName { get; set; }
        public string DocumentTypeAlias { get; set; }
        public int DocumentTypeId { get; set; }
        public int Id { get; set; }
        public PublishedItemType ItemType { get; set; }
        public int Level { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public DateTime UpdateDate { get; set; }
        public string Url { get; set; }
        public string UrlName { get; set; }
        public Guid Version { get; set; }
        public int SortOrder { get; set; }
    }
}
