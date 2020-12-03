using RX6800.Notifier.Library.Helper;
using RX6800.Notifier.Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace RX6800.Notifier.Library.Shop
{
    /// <summary>
    /// Defines the <see cref="Cyberport" />.
    /// </summary>
    public class Cyberport : IWebsite
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Url.
        /// </summary>
        public string Url { get; set; } = "https://www.cyberport.de/pc-und-zubehoer/komponenten/grafikkarten/amd-fuer-gaming.html?productsPerPage=100&sort=popularity&2E_Grafikchip=AMD%20RX%206800%20XT,AMD%20RX%206800&page=1&stockLevelStatus=IMMEDIATELY";

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
                Videocard.RX6800 => "https://www.cyberport.de/pc-und-zubehoer/komponenten/grafikkarten/amd-fuer-gaming.html?productsPerPage=50&sort=popularity&2E_Grafikchip=AMD%20RX%206800&page=1&stockLevelStatus=IMMEDIATELY",
                Videocard.RX6800XT => "https://www.cyberport.de/pc-und-zubehoer/komponenten/grafikkarten/amd-fuer-gaming.html?productsPerPage=50&sort=popularity&2E_Grafikchip=AMD%20RX%206800%20XT&page=1&stockLevelStatus=IMMEDIATELY",
                Videocard.RX6900 => "https://www.cyberport.de/pc-und-zubehoer/komponenten/grafikkarten/amd-fuer-gaming.html?productsPerPage=50&sort=popularity&2E_Grafikchip=AMD%20RX%206900&page=1&stockLevelStatus=IMMEDIATELY",
                _ => Url,
            };
        }

        /// <summary>
        /// Get the stock.
        /// </summary>
        /// <returns>The <see cref="Stock"/>.</returns>
        public Stock GetStock()
        {
            Dictionary<Videocard, int> values = new Dictionary<Videocard, int>();
            GetStock(Videocard.RX6800, "RX 6800", values);
            GetStock(Videocard.RX6800XT, "RX 6800 XT", values);
            GetStock(Videocard.RX6900, "RX 6900", values);
            return new Stock(this, values);
        }

        #endregion

        #region Private

        /// <summary>
        /// Get the stock.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="values">The values<see cref="Dictionary{Videocard, int}"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        private void GetStock(Videocard card, string name, Dictionary<Videocard, int> values)
        {
            string html = GetHTML(GetProductUrl(card)).Result;

            try
            {
                html = html.Replace(@"\", string.Empty);
                var splittedHtml = html.Split("<article class=\" productArticle\"");
                var filteredByName = splittedHtml.Where(o => o.Contains(name)).ToList();
                if (card == Videocard.RX6800)
                    filteredByName = filteredByName.Where(o => !o.Contains("RX 6800 XT")).ToList();
                var filtered = filteredByName.Where(o => !o.Contains("DOCTYPE") && o.Contains("Sofort verfügbar")).ToList();
                values.Add(card, filtered.Count());
            }
            catch (Exception)
            { }
        }

        /// <summary>
        /// The GetHTML.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{string}"/>.</returns>
        private static async Task<string> GetHTML(string url)
        {
            var handler = new HttpClientHandler
            {
                UseCookies = true,
                AutomaticDecompression = ~DecompressionMethods.None
            };
            using (var httpClient = new HttpClient(handler))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), url))
                {
                    request.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:83.0) Gecko/20100101 Firefox/83.0");
                    request.Headers.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                    request.Headers.TryAddWithoutValidation("Accept-Language", "en-US,en;q=0.5");
                    request.Headers.TryAddWithoutValidation("Connection", "keep-alive");
                    request.Headers.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
                    var response = await httpClient.SendAsync(request);
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }

        #endregion
    }
}
