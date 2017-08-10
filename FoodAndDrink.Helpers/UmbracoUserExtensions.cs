using FoodAndDrink.Const;
using System.Linq;
using Umbraco.Core.Models.Membership;

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
}
