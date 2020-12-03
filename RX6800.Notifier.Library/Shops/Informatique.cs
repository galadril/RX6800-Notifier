using RX6800.Notifier.Library.Helper;
using RX6800.Notifier.Library.Model;
using System;
using System.Collections.Generic;

namespace RX6800.Notifier.Library.Shop
{
    /// <summary>
    /// Defines the <see cref="Informatique" />.
    /// </summary>
    public class Informatique : IWebsite
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Url.
        /// </summary>
        public string Url { get; set; } = "https://www.informatique.nl/?m=sts&g=166&p=&sort=&ss=2&pr_min=&pr_max=";

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
                Videocard.RTX3060TI => "https://www.informatique.nl/zoeken/?q=3060&k=20201125&g=166", //Could be improved.. but i dont know the product id
                Videocard.RTX3070 => "https://www.informatique.nl/zoeken/?q=3070&k=20201125&g=166", //Could be improved.. but i dont know the product id
                Videocard.RTX3080 => "https://www.informatique.nl/zoeken/?q=3080&k=20201125&g=166", //Could be improved.. but i dont know the product id
                Videocard.RTX3090 => "https://www.informatique.nl/?m=sts&g=166&p=&sort=&ss=2&pr_min=&pr_max=&at529=27549",
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

            try
            {
                html = html.Split(new string[] { "<h3>Chipset (GPU)</h3><ul><li>" }, StringSplitOptions.None)[1];
                html = html.Split(new string[] { "</ul>" }, StringSplitOptions.None)[0];
            }
            catch
            {}

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
            string str = "RTX ";

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
            if (html != "")
            {
                try
                {
                    str = html.Split(new string[] { str + "  <span class=\"attr_count\">(" }, StringSplitOptions.None)[1];
                    str = str.Split(new string[] { ")" }, StringSplitOptions.None)[0];
                    return int.Parse(str);
                }
                catch (Exception)
                {
                    return 0;
                }
            }
            else
            {
                Logger.HtmlStockCheckError(this);
                return -1;
            }
        }

        #endregion
    }
}
