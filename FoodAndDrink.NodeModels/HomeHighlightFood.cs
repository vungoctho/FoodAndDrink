using Our.Umbraco.Ditto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;

namespace FoodAndDrink.NodeModels
{
    public class HomeHighlightFood : Node
    {
        [UmbracoProperty("FoodImage")]
        public IPublishedContent FoodImage { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }
    }
}
