using FoodAndDrink.Const;
using System;
using System.Linq;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Models.Membership;
using Umbraco.Core.Security;

namespace FoodAndDrink.Helpers
{
    public static class UmbracoUserExtensions
    {
        public static bool IsOfType(this IUser user, params string[] typeAliases)
        {
            if (typeAliases == null || typeAliases.Length == 0) return false;
            return typeAliases.Contains(user.UserType.Alias);
        }

        public static bool IsEditor(this IUser user)
        {
            return user.IsOfType(UserTypeAlias.Admin, UserTypeAlias.Editor);
        }
    }

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
