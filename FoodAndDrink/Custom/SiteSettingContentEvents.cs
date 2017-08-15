using FoodAndDrink.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models;

namespace FoodAndDrink.Custom
{
    public class SiteSettingContentEvents : ContentHandlerBase
    {
        public SiteSettingContentEvents() : base("fADSiteSettings,fADEmailTemplate") { }

        protected override void PostPublished(IContent content)
        {
            SiteMaintenance.Current.Refresh();
        }
    }
}