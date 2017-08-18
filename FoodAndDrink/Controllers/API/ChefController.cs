using FoodAndDrink.Const;
using FoodAndDrink.Controllers.API.Base;
using FoodAndDrink.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;
using Umbraco.Core;

namespace FoodAndDrink.Controllers.API
{
    public class ChefController : ApiControllerBase, IResultApiController
    {
        [HttpPost]
        public ApiResult<int> RegisterNewChef(ChefModel model)
        {
            string errorMessage;
            //if (!IsValidData(model, out errorMessage))
            //{
            //    return GenerateResultError<int>(errorMessage);
            //}

            var password = Membership.GeneratePassword(Membership.MinRequiredPasswordLength, Membership.MinRequiredNonAlphanumericCharacters);

            
            var nodeId = RegisterMemberToBackOffice(model, password);
            
            
                //await SendNewStaffCreatedNotification(model.Staff, password);
                //await SendNewStaffCreatedNotificationToSiebaStaff(model.Staff, school.Name);
            

            return ResultOf(()=>{ return nodeId; } , "Register new chef successful!");
        }

        private int RegisterMemberToBackOffice(ChefModel chef, string password)
        {
            try
            {
                var newMember = ApplicationContext.Services.MemberService.CreateMember(chef.Email, chef.Email, chef.FullName, BackOfficeMemberTypes.Member);
                newMember.SetValue(ChefMemberProfiles.FirstName.ToSafeAlias(true), chef.FirstName);
                newMember.SetValue(ChefMemberProfiles.LastName.ToSafeAlias(true), chef.LastName);
                newMember.SetValue(ChefMemberProfiles.Email.ToSafeAlias(true), chef.Email);
                newMember.SetValue(ChefMemberProfiles.Phone.ToSafeAlias(true), chef.Phone);
                ApplicationContext.Services.MemberService.Save(newMember);
                if (password != null)
                {
                    ApplicationContext.Services.MemberService.SavePassword(newMember, password);
                }
                
                ApplicationContext.Services.MemberService.AssignRole(newMember.Id, UmbracoGroup.Chef);
                
                return newMember.Id;
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to create new member " + ex.Message);
            }
        }
    }
}
