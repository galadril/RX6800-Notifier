using RX6800.Notifier.Library.Model;
using System;

namespace RX6800.Notifier.Library.Helper
{
    /// <summary>
    /// Defines the <see cref="Logger" />.
    /// </summary>
    public static class Logger
    {
        #region Public

        /// <summary>
        /// Log a json error.
        /// </summary>
        /// <param name="field">The field<see cref="string"/>.</param>
        public static void JsonReadError(string field)
        {
            string log = $"Error reading {field} name from json";
            Console.WriteLine(log);
            Mailer.SendLogThreaded(log);
        }

        /// <summary>
        /// Log an error about sending an email.
        /// </summary>
        /// <param name="email">The email<see cref="string"/>.</param>
        public static void EmailError(string email)
        {
            string log = $"Error sending email to: {email}";
            Console.WriteLine(log);
        }

        /// <summary>
        /// Email has been send logging.
        /// </summary>
        /// <param name="email">The email<see cref="string"/>.</param>
        public static void EmailSend(string email)
        {
            string log = $"Email send to: {email}";
            Console.WriteLine(log);
        }

        /// <summary>
        /// Log an error about downloading the html.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        public static void HtmlDownloadError(string url)
        {
            string log = $"Error downloading html with GET: {url}";
            Console.WriteLine(log);
            Mailer.SendLogThreaded(log);
        }

        /// <summary>
        /// Log the new stock values.
        /// </summary>
        /// <param name="stock">The stock<see cref="Stock"/>.</param>
        /// <param name="videocard">The videocard<see cref="Videocard"/>.</param>
        public static void StockUpdate(Stock stock, Videocard videocard)
        {
            string log = $"De voorraad van {Enum.GetName(typeof(Videocard), videocard)} is aangevuld bij {stock.Website.GetType().Name}";
            Console.WriteLine(log);
        }

        /// <summary>
        /// Log an error about retrieveing the new stock values.
        /// </summary>
        /// <param name="website">The website<see cref="IWebsite"/>.</param>
        public static void HtmlStockCheckError(IWebsite website)
        {
            string log = $"Error checking html for stock at: {website.GetType().Name}";
            Console.WriteLine(log);
            Mailer.SendLogThreaded(log);
        }

        #endregion
    }
}
