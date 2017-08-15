using System.Net.Mail;

namespace FoodAndDrink.Helpers.Models
{
    public class MailMessageModel
    {
        public MailMessageModel()
        {
            IsBodyHTML = true;
        }
        public string[] To { get; set; }
        public string[] CC { get; set; }
        public string[] Bcc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public Attachment[] Attachments { get; set; }
        public bool IsBodyHTML { get; set; }
    }
}
