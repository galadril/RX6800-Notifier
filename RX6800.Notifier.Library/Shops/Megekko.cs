using RX6800.Notifier.Library.Helper;
using RX6800.Notifier.Library.Model;
using System;
using System.Collections.Generic;

namespace RX6800.Notifier.Library.Shop
{
    /// <summary>
    /// Defines the <see cref="Megekko" />.
    /// </summary>
    public class Megekko : IWebsite
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Url.
        /// </summary>
        public string Url { get; set; } = "https://www.megekko.nl/Computer/Componenten/Videokaarten/Nvidia-Videokaarten?f=f_vrrd-3_s-prijs09_pp-50_p-1_d-list_cf-";

        #endregion

        #region Public

        /// <summary>
        /// The direct product url.
        /// </summary>
        /// <param name="card">The card<see cref="Videocard"/>.</param>
        /// /// The direct product url.
        public string GetProductUrl(Videocard card)
        {
            return card switch
            {
                Videocard.RTX3060TI => "https://www.megekko.nl/Computer/Componenten/Videokaarten/Nvidia-Videokaarten/Graphics-Engine/GeForce-RTX-3060?f=f_vrrd-0_s-populair_pp-50_p-1_d-list_cf-",
                Videocard.RTX3070 => "https://www.megekko.nl/Computer/Componenten/Videokaarten/Nvidia-Videokaarten/Graphics-Engine/GeForce-RTX-3070?f=f_vrrd-0_s-populair_pp-50_p-1_d-list_cf-",
                Videocard.RTX3080 => "https://www.megekko.nl/Computer/Componenten/Videokaarten/Nvidia-Videokaarten/Graphics-Engine/GeForce-RTX-3080?f=f_vrrd-1_s-populair_pp-50_p-1_d-list_cf-",
                Videocard.RTX3090 => "https://www.megekko.nl/Computer/Componenten/Videokaarten/Nvidia-Videokaarten/Graphics-Engine/GeForce-RTX-3090?f=f_vrrd-1_s-populair_pp-50_p-1_d-list_cf-",
                _ => Url,
            };
        }

        /// <summary>
        /// Get the stock.
        /// </summary>
        /// <returns>The <see cref="Stock"/>.</returns>
        public Stock GetStock()
        {
            string html = WebsiteDownloader.GetHtml(this.Url);
            Dictionary<Videocard, int> values = new Dictionary<Videocard, int>();

            foreach (Videocard card in Enum.GetValues(typeof(Videocard)))
            {
                values[card] = this.CheckHtmlForStock(html, card);
            }

            return new Stock(this, values);
        }

        #endregion

        #region Private

        /// <summary>
        /// Parse the html to retrieve the stock.
        /// </summary>
        /// <param name="html">The html<see cref="string"/>.</param>
        /// <param name="card">The card<see cref="Videocard"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        private int CheckHtmlForStock(string html, Videocard card)
        {
            string str = "GeForce RTX ";

            switch (card)
            {
                case Videocard.RTX3060TI:
                    str += "3060 Ti";
                    break;
                case Videocard.RTX3070:
                    str += "3070";
                    break;
                case Videocard.RTX3080:
                    str += "3080";
                    break;
                case Videocard.RTX3090:
                    str += "3090";
                    break;
            }

            try
            {
                str = html.Split(new string[] { str + " <span" }, StringSplitOptions.None)[1];
                str = str.Split(new string[] { ">(" }, StringSplitOptions.None)[1];
                str = str.Split(new string[] { ")</span>" }, StringSplitOptions.None)[0];
                return int.Parse(str);
            }
            catch (Exception)
            {
                Logger.HtmlStockCheckError(this);
                return -1;
            }
        }

        #endregion
    }
}
