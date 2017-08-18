using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodAndDrink.Models
{
    public class ChefModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public string Phone { get; set; }

        public string FullName { get { return $"{FirstName} {LastName}"; } }
    }
}