using FoodAndDrink.Const;
using FoodAndDrink.Controllers.API.Base;
using FoodAndDrink.Models;
using System;
using System.Web.Http;
using Umbraco.Web.Mvc;

namespace FoodAndDrink.Controllers.API
{
    public class ManageContentController : AuthorisedApiControllerBase, IResultApiController
    {
        [UmbracoAuthorize]
        [UmbracoAuthorizeFor(UserTypeAlias.Admin, UserTypeAlias.Editor)]
        [HttpPost]
        public ApiResult InlineUpdate(InlineRichtextEditor model)
        {
            return ResultOf(() =>
            {
                var service = ApplicationContext.Services.ContentService;
                var node = service.GetById(model.NodeId);
                if (node == null) throw new Exception($"INLINE UPDATE: NodeId = {model.NodeId} is not found.");

                node.Properties[model.PropertyAlias].Value = model.Html;
                service.SaveAndPublishWithStatus(node);
            });
        }
    }
}
