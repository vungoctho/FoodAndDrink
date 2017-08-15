using System;
using System.Collections.Generic;

namespace FoodAndDrink.NodeModels
{
    public class FoodOrderEntry : Node
    {
        public string FullName { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string City { get; set; }

        public string Email { get; set; }

        public DateTime SubmittedDateTime { get; set; }

        [FoodOrderDetailEntryProcessor]
        public List<FoodOrderDetailEntry> OrderDetails { get; set; }
    }
}
