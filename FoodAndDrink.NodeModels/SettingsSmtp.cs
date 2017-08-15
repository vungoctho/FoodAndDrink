using Our.Umbraco.Ditto;
using Umbraco.Core.Models;

namespace FoodAndDrink.NodeModels
{
    public class SettingsSmtp
    {
        [UmbracoProperty("sMTPHost")]
        public string Host { get; set; }

        [UmbracoProperty("sMTPPort")]
        public int Port { get; set; }

        [UmbracoProperty("sMTPUser")]
        public string User { get; set; }

        [UmbracoProperty("sMTPPassword")]
        public string Password { get; set; }

        public string FromEmailAddress { get; set; }
    }

    public class SettingsSmtpProcessorAttribute : DittoProcessorAttribute
    {
        public override object ProcessValue()
        {
            var content = Value as IPublishedContent;
            if (content == null) return null;

            return content.As<SettingsSmtp>();
        }
    }
}
