using FoodAndDrink.Helpers.Models;
using FoodAndDrink.NodeModels;
using System;
using System.ComponentModel;
using System.Net.Mail;
using System.Threading.Tasks;
using Umbraco.Core.Logging;

namespace FoodAndDrink.Helpers
{
    public class EmailSender
    {
        public static void Send(MailMessageModel model, SettingsSmtp smtp)
        {
            var mail = BuildMailMessage(model, smtp.FromEmailAddress);
            try
            {
                using (var client = CreateSmtpClient(smtp))
                {
                    client.Send(mail);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(typeof(EmailSender), $"Host:{smtp.Host} - Port:{smtp.Port} - User: {smtp.User}. {ex.Message}"  , ex);
            }
        }

        public static async Task SendAsync(MailMessageModel model, SettingsSmtp smtp)
        {
            var mail = BuildMailMessage(model, smtp.FromEmailAddress);
            await Task.Run(() =>
            {
                //Host and From are being inherited from MailSetting in web.config
                var client = CreateSmtpClient(smtp);
                //wire up the event for when the Async send is completed
                client.SendCompleted += new SendCompletedEventHandler(SmtpClient_OnCompleted);
                client.SendCompleted += (s, e) =>
                {
                    client.Dispose();
                    mail.Dispose();
                };
                client.SendAsync(mail, mail);
            });

        }

        private static SmtpClient CreateSmtpClient(SettingsSmtp smtp)
        {
            var client = new SmtpClient(smtp.Host, smtp.Port);
            if (!string.IsNullOrEmpty(smtp.User) && !string.IsNullOrEmpty(smtp.Password))
            {
                client.Credentials = new System.Net.NetworkCredential(smtp.User, smtp.Password);
            }
            return client;
        }

        private static MailMessage BuildMailMessage(MailMessageModel model, string fromEmailAddress = "operationsmanager@fad.com")
        {
            var mail = new MailMessage()
            {
                Subject = model.Subject,
                Body = model.Body,
                IsBodyHtml = model.IsBodyHTML
            };
            if (model.To != null)
            {
                foreach (var item in model.To)
                {
                    if (!string.IsNullOrEmpty(item))
                        mail.To.Add(item);
                }
            }

            if (model.CC != null)
            {
                foreach (var item in model.CC)
                {
                    if (!string.IsNullOrEmpty(item))
                        mail.CC.Add(item);
                }
            }

            if (model.Bcc != null)
            {
                foreach (var item in model.Bcc)
                {
                    if (!string.IsNullOrEmpty(item))
                        mail.Bcc.Add(item);
                }
            }

            if (model.Attachments != null)
            {
                foreach (var att in model.Attachments)
                {
                    mail.Attachments.Add(att);
                }
            }
            mail.From = new MailAddress(fromEmailAddress);
            return mail;
        }

        private static void SmtpClient_OnCompleted(object sender, AsyncCompletedEventArgs e)
        {
            //Get the Original MailMessage object
            MailMessage mail = (MailMessage)e.UserState;
            string msg = "Sending {0} has {1}";

            if (e.Cancelled)
            {
                LogHelper.Warn(typeof(EmailSender), string.Format(msg, mail.Subject, "been canceled."));
            }
            if (e.Error != null)
            {
                LogHelper.Error(typeof(EmailSender), string.Format(msg, mail.Subject, "error."), e.Error);
            }
            else
            {
                LogHelper.Info(typeof(EmailSender), string.Format(msg, mail.Subject, "been sent sucessfully."));
            }

        }
    }
}
