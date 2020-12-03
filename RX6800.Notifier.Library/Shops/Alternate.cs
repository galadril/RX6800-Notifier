using RX6800.Notifier.Library.Helper;
using RX6800.Notifier.Library.Model;
using System;
using System.Collections.Generic;

namespace RX6800.Notifier.Library.Shop
{
    /// <summary>
    /// Defines the <see cref="Alternate" />.
    /// </summary>
    public class Alternate : IWebsite
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Url.
        /// </summary>
        public string Url { get; set; } = "https://www.alternate.nl/Grafische-kaarten/GeForce-RTX-Gaming/html/listings/1534500258044?lk=21501&showFilter=false&hideFilter=false&disableFilter=false&filter_-1=31900&filter_-1=209900&filter_-2=true";

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
                Videocard.RTX3060TI => "https://www.alternate.nl/Grafische-kaarten/RTX-3060-ti/html/listings/1599465706202?lk=29309&sort=AVAILABILITY&order=ASC&hideFilter=false&filter_-2=true",
                Videocard.RTX3070 => "https://www.alternate.nl/Grafische-kaarten/RTX-3070/html/listings/1599465706202?lk=29309&sort=AVAILABILITY&order=ASC&hideFilter=false&filter_-2=true",
                Videocard.RTX3080 => "https://www.alternate.nl/Grafische-kaarten/RTX-3080/html/listings/1599465397714?lk=29308&sort=AVAILABILITY&order=ASC&hideFilter=false&filter_-2=true",
                Videocard.RTX3090 => "https://www.alternate.nl/Grafische-kaarten/RTX-3090/html/listings/1599465355613?lk=29307&sort=AVAILABILITY&order=ASC&hideFilter=false&filter_-2=true",
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
            Console.WriteLine(html);
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
            string str = "NVIDIA GeForce RTX ";

            switch (card)
            {
                case Videocard.RTX3060TI:
                    str += "3060";
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
                str = html.Split(new string[] { str + "&nbsp;(" }, StringSplitOptions.None)[1];
                str = str.Split(new string[] { ")" }, StringSplitOptions.None)[0];
                return int.Parse(str);
            }
            catch (Exception)
            {
                return -1;
            }
        }

        #endregion
    }
}
