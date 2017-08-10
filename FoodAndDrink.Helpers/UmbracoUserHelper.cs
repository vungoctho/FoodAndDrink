using FoodAndDrink.Const;
using System;
using System.Linq;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Models.Membership;
using Umbraco.Core.Security;

namespace FoodAndDrink.Helpers
{
    public class UmbracoUserHelper
    {
        private static Lazy<UmbracoUserHelper> _instance = new Lazy<UmbracoUserHelper>();
        public static UmbracoUserHelper Current => _instance.Value;

        public IUser User
        {
            get
            {
                var ticket = new HttpContextWrapper(HttpContext.Current).GetUmbracoAuthTicket();
                if (ticket == null) return null;
                return ApplicationContext.Current.Services.UserService.GetByUsername(ticket.Name);
            }
        }
    }
}
