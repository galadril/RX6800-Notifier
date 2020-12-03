using RX6800.Notifier.Library.Model;
using System;
using System.Collections.Generic;

namespace RX6800.Notifier.Library.Helper
{
    /// <summary>
    /// Defines the <see cref="Printer" />.
    /// </summary>
    public static class Printer
    {
        #region Public

        /// <summary>
        /// Print the new stock values.
        /// </summary>
        /// <param name="stock">The stock<see cref="Stock"/>.</param>
        public static void PrintStock(Stock stock)
        {
            string toPrint = stock.Website.GetType().Name + " -- " + stock.Timestamp.ToString();

            foreach (KeyValuePair<Videocard, int> entry in stock.Values)
            {
                toPrint += $"\n{Enum.GetName(typeof(Videocard), entry.Key)} : {entry.Value}";
            }

            Console.WriteLine(toPrint + "\n------------------------");
        }

        /// <summary>
        /// Print the subscriber info.
        /// </summary>
        /// <param name="subscriber">The subscriber<see cref="Subscriber"/>.</param>
        public static void PrintSubscriber(Subscriber subscriber)
        {
            string toPrint = "Subscriber\n";

            toPrint += $"Email : {subscriber.Email}\nInterested in:";

            foreach (Videocard videocard in subscriber.Interests)
            {
                toPrint += $"\n{Enum.GetName(typeof(Videocard), videocard)}";
            }

            Console.WriteLine(toPrint + "\n------------------------");
        }

        #endregion
    }
}
