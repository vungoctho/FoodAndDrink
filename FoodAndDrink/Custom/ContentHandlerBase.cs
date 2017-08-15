using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Publishing;
using Umbraco.Core.Services;

namespace FoodAndDrink.Custom
{
    public abstract class ContentHandlerBase : ApplicationEventHandler
    {
        protected string[] ContentAlias { get; private set; }

        public ContentHandlerBase(string alias)
        {
            ContentAlias = alias.Split(',');
        }

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            ContentService.Published += ContentService_Published;
        }

        private void ContentService_Published(IPublishingStrategy sender, PublishEventArgs<IContent> e)
        {
            foreach (var content in e.PublishedEntities)
            {
                if (content.ContentType.Alias.ContainsAny(ContentAlias))
                {
                    PostPublished(content);
                }
            }
        }

        protected abstract void PostPublished(IContent content);
    }
}