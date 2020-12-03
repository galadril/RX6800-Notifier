using RestSharp;
using RX6800.Notifier.Library.Helper;
using RX6800.Notifier.Library.Model;
using System;
using System.Collections.Generic;

namespace RX6800.Notifier.Library.Shop
{
    /// <summary>
    /// Defines the <see cref="Cdromland" />.
    /// </summary>
    public class Cdromland : IWebsite
    {
        #region Variables

        /// <summary>
        /// Defines the url.
        /// </summary>
        private string url = "https://www.cdromland.nl/setfilter.php";

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Url.
        /// </summary>
        public string Url { get; set; } = "https://cdromland.nl/";

        #endregion

        #region Public

        /// <summary>
        /// The direct product url.
        /// </summary>
        /// <param name="card">The card<see cref="Videocard"/>.</param>
        /// /// The direct product url.
        public string GetProductUrl(Videocard card)
        {
            return Url;
        }

        /// <summary>
        /// Get the stock.
        /// </summary>
        /// <returns>The <see cref="Stock"/>.</returns>
        public Stock GetStock()
        {
            string html = DownloadHtml();

            Dictionary<Videocard, int> values2 = new Dictionary<Videocard, int>();

            foreach (Videocard card in Enum.GetValues(typeof(Videocard)))
            {
                values2[card] = this.CheckHtmlForStock(html, card);
            }

            return new Stock(this, values2);
        }

        #endregion

        #region Private

        /// <summary>
        /// Download html content.
        /// </summary>
        private string DownloadHtml()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>() { { "main", "20" }, { "sub", "3" }, { "levertijd", "3" } };
            string result = "";
            try
            {
                var client = new RestClient(this.url);
                client.AddDefaultHeader("Content-type", "application/x-www-form-urlencoded");
                var request = new RestRequest();
                request.AddHeader("Content-type", "application/x-www-form-urlencoded");

                foreach (KeyValuePair<string, string> pair in parameters)
                {
                    request.AddParameter(pair.Key, pair.Value);
                }

                IRestResponse response = client.Post(request);
                result = response.Content;
            }
            catch (Exception)
            {
                Logger.HtmlDownloadError(url);
            }
            return result;
        }

        /// <summary>
        /// Parse the html to retrieve the stock.
        /// </summary>
        /// <param name="html">The html<see cref="string"/>.</param>
        /// <param name="card">The card<see cref="Videocard"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        private int CheckHtmlForStock(string html, Videocard card)
        {
            string str = "AMD RX ";

            switch (card)
            {
                case Videocard.RX6800:
                    str += "6800";
                    break;
                case Videocard.RX6800XT:
                    str += "6800 XT";
                    break;
                case Videocard.RX6900:
                    str += "6900";
                    break;
            }

            if (html != "")
            {
                try
                {
                    str = html.Split(new string[] { str + "&nbsp;&nbsp;(" }, StringSplitOptions.None)[1].ToString();
                    str = str.Split(new string[] { ")</label>" }, StringSplitOptions.None)[0].ToString();
                    return int.Parse(str);
                }
                catch
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
