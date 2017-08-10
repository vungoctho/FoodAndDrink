using FoodAndDrink.Helpers;
using FoodAndDrink.Models;
using System;
using Umbraco.Web.WebApi;

namespace FoodAndDrink.Controllers.API.Base
{
    public abstract class ApiControllerBase : UmbracoApiController
    {
        /// <summary>
        /// No return values
        /// </summary>
        /// <param name="action"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public ApiResult ResultOf(Action action, params string[] messages)
        {
            return ApiResultHelper.ResultOf(action, messages);
        }

        /// <summary>
        /// Strongly-typed return values
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="action"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public ApiResult<TData> ResultOf<TData>(Func<TData> action, params string[] messages)
        {
            return ApiResultHelper.ResultOf(action, messages);
        }

        public ApiResult GenerateResultError(params string[] messages)
        {
            return new ApiResult
            {
                Success = false,
                Messages = messages,
                Data = null
            };
        }

        public ApiResult<TData> GenerateResultError<TData>(params string[] messages)
        {
            return new ApiResult<TData>
            {
                Success = false,
                Messages = messages,
                Data = default(TData)
            };
        }
    }
}