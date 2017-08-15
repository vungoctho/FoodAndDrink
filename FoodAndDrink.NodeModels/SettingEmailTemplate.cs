using Our.Umbraco.Ditto;
using System.Linq;
using Umbraco.Core.Models;

namespace FoodAndDrink.NodeModels
{
    public class SettingEmailTemplate : Node
    {
        public string FoodOrderSubject { get; set; }

        public string FoodOrderBody { get; set; }

        public string FoodOrderBCC { get; set; }
    }

    public class SettingEmailTemplateProcessorAttribute : DittoProcessorAttribute
    {
        public override object ProcessValue()
        {
            var content = Value as IPublishedContent;
            if (content == null) return null;

            return content.Children.Where(x => x.DocumentTypeAlias == "fADEmailTemplate")
                .OrderBy(x => x.SortOrder)
                .Select(x => x.As<SettingEmailTemplate>())
                .FirstOrDefault();
        }
    }
}
