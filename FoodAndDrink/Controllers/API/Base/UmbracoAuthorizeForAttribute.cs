using FoodAndDrink.Helpers;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace FoodAndDrink.Controllers.API.Base
{
    public class UmbracoAuthorizeForAttribute : AuthorizeAttribute
    {
        public string[] AllowUserTypes { get; set; }

        public UmbracoAuthorizeForAttribute(params string[] allowUserTypes)
        {
            AllowUserTypes = allowUserTypes;
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            if (base.IsAuthorized(actionContext))
            {
                return UmbracoUserHelper.Current.User.IsOfType(AllowUserTypes);
            }

            return false;
        }
    }
}