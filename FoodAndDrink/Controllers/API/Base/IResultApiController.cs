using FoodAndDrink.Models;
using System;

namespace FoodAndDrink.Controllers.API.Base
{
    public interface IResultApiController
    {
        ApiResult ResultOf(Action action, params string[] messages);
        ApiResult<TData> ResultOf<TData>(Func<TData> action, params string[] messages);
        ApiResult GenerateResultError(params string[] messages);
        ApiResult<TData> GenerateResultError<TData>(params string[] messages);
    }
}