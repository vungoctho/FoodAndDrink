using FoodAndDrink.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Umbraco.Core.Logging;

namespace FoodAndDrink.Helpers
{
    public static class ApiResultHelper
    {
        private static Type GetCallingType()
        {
            var stackTrace = new StackTrace();
            Type type = null;

            for (var i = 0; i < stackTrace.FrameCount; i++)
            {
                type = stackTrace.GetFrame(i).GetMethod().DeclaringType;
                if (!type.IsAbstract && type != typeof(ApiResultHelper)) break;
            }
            return type != null ? type : typeof(ApiResultHelper);
        }

        private static ApiResult Ok(object data = null, params string[] messages)
        {
            if (messages == null) { messages = new[] { "Your request has been processed successfully." }; }
            return new ApiResult
            {
                Messages = messages,
                Data = data
            };
        }

        private static ApiResult<TData> Ok<TData>(TData data = default(TData), params string[] messages)
        {
            if (messages == null) { messages = new[] { "Your request has been processed successfully." }; }
            return new ApiResult<TData>
            {
                Messages = messages,
                Data = data
            };
        }

        private static ApiResult Error(Exception ex, object data = null)
        {
            var clientMessage = "An error occurs when processing your request. Please try again.";

            LogHelper.Error(GetCallingType(), ex.Message, ex);
            return new ApiResult
            {
                Success = false,
                Messages = new[] { clientMessage },
                Data = data
            };
        }

        private static ApiResult<TData> Error<TData>(Exception ex, TData data = default(TData))
        {
            var clientMessage = "An error occurs when processing your request. Please try again.";

            LogHelper.Error(GetCallingType(), ex.Message, ex);
            return new ApiResult<TData>
            {
                Success = false,
                Messages = new[] { clientMessage },
                Data = data
            };
        }

        private static ApiResult Error(string message, object data = null)
        {
            return Error(new Exception(message), data);
        }

        private static ApiResult<TData> Error<TData>(string message, TData data = default(TData))
        {
            return Error(new Exception(message), data);
        }

        /// <summary>
        /// No return values
        /// </summary>
        /// <param name="action"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ApiResult ResultOf(Action action, params string[] messages)
        {
            try
            {
                action?.Invoke();
                return Ok(messages);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        /// <summary>
        /// Strongly-typed return values
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="action"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ApiResult<TData> ResultOf<TData>(Func<TData> action, params string[] messages)
        {
            try
            {
                return Ok(action != null ? action() : default(TData), messages);
            }
            catch (Exception ex)
            {
                return Error(ex, default(TData));
            }
        }
    }
}