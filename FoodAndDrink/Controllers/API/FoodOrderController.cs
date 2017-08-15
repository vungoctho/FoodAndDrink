using FoodAndDrink.Const;
using FoodAndDrink.Controllers.API.Base;
using FoodAndDrink.Helpers;
using FoodAndDrink.Helpers.Models;
using FoodAndDrink.Models;
using FoodAndDrink.NodeModels;
using Newtonsoft.Json.Linq;
using Our.Umbraco.Ditto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Umbraco.Core;
using Umbraco.Core.Logging;

namespace FoodAndDrink.Controllers.API
{
    public class FoodOrderController : ApiControllerBase, IResultApiController
    {
        [HttpPost]
        public ApiResult Post([FromBody] JObject data)
        {
            var orderEntry = data["record"].ToObject<FoodOrderEntry>();            
            orderEntry.SubmittedDateTime = DateTime.Now;

            //Validate the object
            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(orderEntry, new ValidationContext(orderEntry), validationResults, true))
            {
                //Concatenate the errors message and throw to client
                return GenerateResultError(validationResults.Select(x => x.ErrorMessage).ToArray());
            }

            //If we got this far then the object is valid, then we can create the content
            var orderId = SaveOrderEntryToUmbraco(orderEntry);
            SendEmail(orderEntry, orderId);

            return ResultOf(() =>
            {
                return true;

            }, "You order has been sent and we will be in touch as soon as we can.");
        }

        private int SaveOrderEntryToUmbraco(FoodOrderEntry orderEntry)
        {
            //check if folder is not exist, create it
            var todayOrderFolder = ContentQuery.GetOrderFolder(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            if (todayOrderFolder == null)
            {
                todayOrderFolder = SiteMaintenance.Current.GenerateFolder(ContentQuery.GetFoodOrderListNode(), DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            }
            var nodeName = $"{orderEntry.FullName}_{DateTime.Now.ToString(Strings.BackOfficeDatetimeFormat)}";
            var contentService = ApplicationContext.Services.ContentService;
            var order = contentService.CreateContent(nodeName, todayOrderFolder.Id, DocumentTypeAlias.FADOrderEntry);
            order.SetValue(OrderEntry.FullName.ToSafeAlias(true), orderEntry.FullName);
            order.SetValue(OrderEntry.Address.ToSafeAlias(true), orderEntry.Address);
            order.SetValue(OrderEntry.Email.ToSafeAlias(true), orderEntry.Email);
            order.SetValue(OrderEntry.Phone.ToSafeAlias(true), orderEntry.Phone);
            order.SetValue(OrderEntry.City.ToSafeAlias(true), orderEntry.City);
            order.SetValue(OrderEntry.SubmittedDatetime.ToSafeAlias(true), orderEntry.SubmittedDateTime);
            contentService.SaveAndPublishWithStatus(order);

            if (orderEntry.OrderDetails == null || !orderEntry.OrderDetails.Any()) return order.Id ;
            foreach (var detail in orderEntry.OrderDetails)
            {
                var orderDetail = contentService.CreateContent(detail.FoodName, order.Id, DocumentTypeAlias.FADOrderDetailEntry);
                orderDetail.SetValue(OrderDetailEntry.FoodId.ToSafeAlias(true), detail.FoodId);
                orderDetail.SetValue(OrderDetailEntry.FoodName.ToSafeAlias(true), detail.FoodName);
                orderDetail.SetValue(OrderDetailEntry.FoodDescription.ToSafeAlias(true), detail.FoodDescription);
                orderDetail.SetValue(OrderDetailEntry.Price.ToSafeAlias(true), detail.Price);
                orderDetail.SetValue(OrderDetailEntry.Amount.ToSafeAlias(true), detail.Amount);
                contentService.SaveAndPublishWithStatus(orderDetail);
            }
            return order.Id;
        }

        //TODO: Mon 14/8: check & test send email with smtp gmail
        private void SendEmail(FoodOrderEntry orderEntry, int orderId)
        {
            var siteSetting = SiteMaintenance.SettingsNode.As<SiteSettings>(); ;
            var emailTemplate = siteSetting.EmailTemplate;
            var bcc = new string[]{ };
            if (!string.IsNullOrEmpty(emailTemplate.FoodOrderBCC))
            {
                bcc = emailTemplate.FoodOrderBCC.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            }
            
            var subject = string.Format(emailTemplate.FoodOrderSubject, orderId);
            var body = string.Format(emailTemplate.FoodOrderBody, orderEntry.FullName, DateTime.Now, orderEntry.Address);
            EmailSender.Send(new MailMessageModel()
            {
                To = new string[] { orderEntry.Email},
                Subject = subject,
                Body = body,
                Bcc = bcc
            }, siteSetting.Smtp);
        }

    }
}
