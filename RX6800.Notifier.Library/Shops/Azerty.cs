using RestSharp;
using RX6800.Notifier.Library.Helper;
using RX6800.Notifier.Library.Model;
using System;
using System.Collections.Generic;

namespace RX6800.Notifier.Library.Shop
{
    /// <summary>
    /// Defines the <see cref="Azerty" />.
    /// </summary>
    public class Azerty : IWebsite
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Url.
        /// </summary>
        public string Url { get; set; } = "https://azerty.nl/category/componenten/videokaarten/nvidia_geforce#!sorting=15&limit=96&view=rows&Videochip_generatie=GeForce_3000&levertijd=green";

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
                Videocard.RTX3060TI => "https://azerty.nl/componenten/videokaarten/nvidia_geforce/nvidia_geforce_rtx_3060_ti#!sorting=12&limit=30&view=grid",
                Videocard.RTX3070 => "https://azerty.nl/componenten/videokaarten/nvidia_geforce/nvidia_geforce_rtx_3070#!sorting=12&limit=30&view=grid",
                Videocard.RTX3080 => "https://azerty.nl/componenten/videokaarten/nvidia_geforce/nvidia_geforce_rtx_3080#!sorting=12&limit=30&view=grid",
                Videocard.RTX3090 => "https://azerty.nl/componenten/videokaarten/nvidia_geforce/nvidia_geforce_rtx_3090#!sorting=12&limit=30&view=grid",
                _ => Url,
            };
        }

        /// <summary>
        /// Get the stock.
        /// </summary>
        /// <returns>The <see cref="Stock"/>.</returns>
        public Stock GetStock()
        {
            string html = DownloadHtml();

            try
            {
                html = html.Replace(@"\", string.Empty);
                html = html.Split(new string[] { "filter_Videochip_videokaarten" }, StringSplitOptions.None)[1];
                html = html.Split(new string[] { "</ul>" }, StringSplitOptions.None)[0];
            }
            catch (Exception)
            {

            }

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
        /// /// The direct product url.
        private string DownloadHtml()
        {
            var client = new RestClient("https://azerty.nl/system/modules/ajax/lib/webservice/load.php");
            client.AddDefaultHeader("content-type", "application/x-www-form-urlencoded");
            var request = new RestRequest();
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("data", "%7B%22service%22%3A%22getFilterOptions%22%2C%22route%22%3A%5B%22lister%22%2C%22componenten%22%2C%22videokaarten+%22%5D%2C%22params%22%3A%7B%22navigation%22%3A%2239%22%2C%22keywords%22%3A%22%22%2C%22keyData%22%3A%22eThVbEhTbWJObW5WcU54TXo4Ky9YZUgwZklIVjFkNGtleml3MFlrQ204S0pPOUt5c2xTQWlLRWdrNHBibm5tU1ZxVVFLZWdPUlZwNU9NUlY4RmhneEdMOW1JNnZxTUlmaEJvYU8yWEtreHR5VjlZTGZGZUgraFlzb2I1dTl6UWoyZkxkZXQrTWZoZGFYNVVaV2xGVFoyS2NFN0JUckVOWkgyUzk4Wk85QnhzPQ%3D%3D%22%7D%2C%22state%22%3A%7B%22sorting%22%3A%2215%22%2C%22limit%22%3A%2230%22%2C%22view%22%3A%22grid%22%7D%2C%22callID%22%3A1%7D");

            var response = client.Post(request);
            return response.Content;
        }

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

            if (html != "")
            {
                try
                {
                    str = html.Split(new string[] { str + "                    <div class=\"count\">(" }, StringSplitOptions.None)[1].ToString();
                    str = str.Split(new string[] { ")</div>" }, StringSplitOptions.None)[0].ToString();
                    return int.Parse(str);
                }
                catch
                {
                    return 0;
                }
            }
            else
            {
                return -1;
            }
        }

        #endregion
    }
}
