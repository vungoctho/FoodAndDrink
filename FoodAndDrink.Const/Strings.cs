using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodAndDrink.Const
{
    public struct UserTypeAlias
    {
        public const string Admin = "admin";
        public const string Editor = "editor";
        public const string Writer = "writer";
    }

    public struct Strings
    {
        public const string BackOfficeDatetimeFormat = "yyyy-MM-dd-HHmmss";
    }

    public struct DocumentTypeAlias
    {
        public const string FADOrderEntry = "fADOrderEntry";
        public const string FADOrderDetailEntry = "fADOrderDetailEntry";
    }

    public struct OrderEntry
    {
        public const string FullName = "FullName";
        public const string Address = "Address";
        public const string Email = "Email";
        public const string Phone = "Phone";
        public const string City = "City";
        public const string SubmittedDatetime = "Submitted Datetime";
    }

    public struct OrderDetailEntry
    {
        public const string FoodId = "FoodId";
        public const string FoodName = "Food Name";
        public const string FoodDescription = "Food Description";
        public const string Price = "Price";
        public const string Amount = "Amount";
    }
}
