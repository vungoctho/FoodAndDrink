using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodAndDrink.Models
{
    public class ApiResult<TData> : ApiResult
    {
        public new TData Data { get; set; }
    }

    public class ApiResult
    {
        public bool Success { get; set; } = true;
        public int? AffectedRecords { get; set; }
        public string[] Messages { get; set; }
        public object Data { get; set; }

        public string AllMessages
        {
            get
            {
                if (Messages == null || Messages.Length == 0) return null;

                return Messages.Aggregate((x, y) => x.Trim().TrimEnd('.') + ". " + y.Trim().TrimEnd('.'));
            }
        }
    }
}