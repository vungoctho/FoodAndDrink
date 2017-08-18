using System.Web.Mvc;
using Umbraco.Web.Controllers;
using Umbraco.Web.Models;

namespace FoodAndDrink.Controllers.Surface
{
    public class ChefLoginController : UmbLoginController
    {
        [System.Web.Http.HttpPost]
        public ActionResult ChefHandleLogin([Bind(Prefix = "loginModel")]LoginModel model)
        {
            if (ModelState.IsValid == false)
            {
                return CurrentUmbracoPage();
            }

            var findMember = Services.MemberService.GetByUsername(model.Username);

            if (findMember != null && findMember.IsLockedOut)
            {
                var validationMessage = "Your account has been locked due to too many unsuccessful login attempts. Please retry again later.";
                ModelState.AddModelError("loginModel", validationMessage);
                return CurrentUmbracoPage();
            }

            if (Members.Login(model.Username, model.Password) == false)
            {
                ModelState.AddModelError("loginModel", "Invalid username or password");
                return CurrentUmbracoPage();
            }

            //Redirect to previous page if available
            if (Request.UrlReferrer != null)
            {
                return Redirect(Request.UrlReferrer.PathAndQuery);
            }

            //redirect to current page by default
            return RedirectToCurrentUmbracoPage();
        }
    }
}
