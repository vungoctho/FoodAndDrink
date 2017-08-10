using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace FoodAndDrink.Helpers
{
    public static class ContentQuery
    {
        public static IPublishedContent GetRootNode()
        {
            return UmbracoContext.Current.ContentCache.GetAtRoot().FirstOrDefault(x => x.DocumentTypeAlias == "fADSite");
        }

       
        public static IPublishedContent GetFoodOrderListNode()
        {
            var root = GetRootNode();
            return root.Children("fADPageOrderList").FirstOrDefault();
        }

        public static IPublishedContent GetOrderFolder(int year, int month, int day)
        {
            var orderListNode = GetFoodOrderListNode();
            var yearNode = orderListNode.Children(s => s.Name == year.ToString() && s.DocumentTypeAlias == "fADOrderListFolder").FirstOrDefault();
            if (yearNode == null) return null;
            var monthNode = yearNode.Children(s => s.Name == month.ToString("D2") && s.DocumentTypeAlias == "fADOrderListFolder").FirstOrDefault();
            if (monthNode == null) return null;
            var dayNode = monthNode.Children(s => s.Name == day.ToString("D2") && s.DocumentTypeAlias == "fADOrderListFolder").FirstOrDefault();
            return dayNode;
        }

        public static IPublishedContent GetFirstChild(this IPublishedContent node, string alias)
        {
            return node.Children.FirstOrDefault(x => x.DocumentTypeAlias == alias);
        }

        public static IEnumerable<IPublishedContent> GetChildren(this IPublishedContent node, string alias)
        {
            return node.Children.Where(x => x.DocumentTypeAlias == alias);
        }
    }
}
