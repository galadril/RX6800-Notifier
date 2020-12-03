using RX6800.Notifier.Library.Model;
using System;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using Windows.System;
using Windows.UI.Notifications;

namespace RX6800.Notifier.Library.Helper
{
    /// <summary>
    /// Defines the <see cref="Mailer" />.
    /// </summary>
    public static class Mailer
    {
        #region Variables

        /// <summary>
        /// Defines the ProductUrl.
        /// </summary>
        public static String ProductUrl = "";

        #endregion

        #region Public

        /// <summary>
        /// Send notifications threaded.
        /// </summary>
        /// <param name="stock">The stock<see cref="Stock"/>.</param>
        /// <param name="videocard">The videocard<see cref="Videocard"/>.</param>
        public static void SendNotificationsThreaded(Stock stock, Videocard videocard)
        {
            new Thread(() => SendNotifications(stock, videocard)).Start();
        }

        /// <summary>
        /// Send notifications.
        /// </summary>
        /// <param name="stock">The stock<see cref="Stock"/>.</param>
        /// <param name="videocard">The videocard<see cref="Videocard"/>.</param>
        public static void SendNotifications(Stock stock, Videocard videocard)
        {
            if (!Constants.GetUseMongoDb())
                return;
            var subscribers = Mongo.GetSubscribers();
            foreach (Subscriber subscriber in subscribers)
            {
                if (subscriber.Interests.Contains(videocard))
                {
                    SendNotificationThreaded(stock, videocard, subscriber);
                }
            }
        }

        /// <summary>
        /// Send notifications threaded.
        /// </summary>
        /// <param name="stock">The stock<see cref="Stock"/>.</param>
        /// <param name="videocard">The videocard<see cref="Videocard"/>.</param>
        /// <param name="subscriber">The subscriber<see cref="Subscriber"/>.</param>
        public static void SendNotificationThreaded(Stock stock, Videocard videocard, Subscriber subscriber)
        {
            new Thread(() => SendNotification(stock, videocard, subscriber)).Start();
        }

        /// <summary>
        /// Send a single notification.
        /// </summary>
        /// <param name="stock">The stock<see cref="Stock"/>.</param>
        /// <param name="videocard">The videocard<see cref="Videocard"/>.</param>
        /// <param name="subscriber">The subscriber<see cref="Subscriber"/>.</param>
        public static void SendNotification(Stock stock, Videocard videocard, Subscriber subscriber)
        {
            string subject = $"AMDTracker: {Enum.GetName(typeof(Videocard), videocard)}";
            string body = "Beste Lezer,<br><br>" +
                        $"De voorraad van {Enum.GetName(typeof(Videocard), videocard)} is aangevuld bij <a href=\"{stock.Website.GetProductUrl(videocard)}\">{stock.Website.GetType().Name}</a><br><br>" +
                        "Wees er snel bij!<br><br>" +
                        $"<a href=\"https://geforce.nieknijland.com/voorkeuren/{subscriber.Id}\">Emailvoorkeur aanpassen</a>";
            //SendMail(subscriber.Email, subject, body);
        }

        /// <summary>
        /// Send logs threaded.
        /// </summary>
        /// <param name="log">The log<see cref="string"/>.</param>
        public static void SendLogThreaded(string log)
        {
            new Thread(() => SendLog(log)).Start();
        }

        /// <summary>
        /// Send logs.
        /// </summary>
        /// <param name="log">The log<see cref="string"/>.</param>
        public static void SendLog(string log)
        {
            string subject = $"AMDTracker: new log";
            string body = "AMDTracker meld het volgende:<br><br>" + log;

            //SendMail(Constants.GetErrorLogAddress(), subject, body);
        }

        /// <summary>
        /// Send the actual email to the smtp server.
        /// </summary>
        /// <param name="subject">The subject<see cref="string"/>.</param>
        /// <param name="body">The body<see cref="string"/>.</param>
        public static void SendToast(string subject, string body = null)
        {
            if (!Constants.GetUseToasts())
                return;

            var template = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText01);
            var textNodes = template.GetElementsByTagName("text").ToList();
            foreach (var textNode in textNodes)
            {
                textNode.AppendChild(template.CreateTextNode(body));
            }

            var toast = new ToastNotification(template)
            {
                Tag = "RTX 30XX Notifier",
                Group = "RTX30XX",
                ExpirationTime = DateTimeOffset.Now.AddMinutes(5),
            };

            toast.Activated += toast_Activated;
            var notifier = ToastNotificationManager.CreateToastNotifier(subject);
            notifier.Show(toast);
        }

        /// <summary>
        /// The toast_Activated.
        /// </summary>
        /// <param name="sender">The sender<see cref="ToastNotification"/>.</param>
        /// <param name="args">The args<see cref="object"/>.</param>
        public async static void toast_Activated(ToastNotification sender, object args)
        {
            await Launcher.LaunchUriAsync(new Uri(!string.IsNullOrEmpty(ProductUrl) ? ProductUrl : "https://geforce.nieknijland.com/"));
        }

        #endregion

        #region Private

        /// <summary>
        /// Send the actual email to the smtp server.
        /// </summary>
        /// <param name="email">The email<see cref="string"/>.</param>
        /// <param name="subject">The subject<see cref="string"/>.</param>
        /// <param name="body">The body<see cref="string"/>.</param>
        private static void SendMail(string email, string subject, string body)
        {
            try
            {
                var mail = new MailMessage();
                var smtp = new SmtpClient(Constants.GetEmailHost());

                mail.From = new MailAddress(Constants.GetEmailUsername());
                mail.To.Add(email);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

                smtp.Port = Constants.GetEmailPort();
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtp.Credentials = new System.Net.NetworkCredential(Constants.GetEmailUsername(), Constants.GetEmailPassword());
                smtp.EnableSsl = true;

                smtp.Send(mail);
                Logger.EmailSend(email);
            }
            catch (Exception)
            {
                Logger.EmailError(email);
            }
        }

        #endregion
    }
}
