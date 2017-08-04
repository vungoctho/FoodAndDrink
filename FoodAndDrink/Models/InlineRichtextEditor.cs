using System;

namespace FoodAndDrink.Models
{
    public class InlineRichtextEditor
    {
        private static readonly Random rd = new Random();
        public int EditorId => rd.Next(1, 1000);
        public string Html { get; set; }
        public int NodeId { get; set; }
        public string PropertyAlias { get; set; }
        public string UpdateUrl => "/managecontent/inlineupdate";
        public string CustomWrapperClass { get; set; }
    }
}