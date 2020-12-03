using RX6800.Notifier.Library.Helper;
using RX6800.Notifier.Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RX6800.Notifier.Library.Shop
{
    /// <summary>
    /// Defines the <see cref="PCKing" />.
    /// </summary>
    public class PCKing : IWebsite
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Url.
        /// </summary>
        public string Url { get; set; } = "https://www.pcking.de/eshop.php?action=like_search&shopfilter_category=&s_group_id=*&s_order_name=&onlygroups=0&s_available=&articlelist_type=search&s_volltext=3060%2C+3080%2C+3070%2C+3060";

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
                Videocard.RTX3060TI => "https://www.pcking.de/eshop.php?action=like_search&shopfilter_category=&s_group_id=*&s_order_name=&onlygroups=0&s_available=&articlelist_type=search&s_volltext=3060",
                Videocard.RTX3070 => "https://www.pcking.de/eshop.php?action=like_search&shopfilter_category=&s_group_id=*&s_order_name=&onlygroups=0&s_available=&articlelist_type=search&s_volltext=3070",
                Videocard.RTX3080 => "https://www.pcking.de/eshop.php?action=like_search&shopfilter_category=&s_group_id=*&s_order_name=&onlygroups=0&s_available=&articlelist_type=search&s_volltext=3080",
                Videocard.RTX3090 => "https://www.pcking.de/eshop.php?action=like_search&shopfilter_category=&s_group_id=*&s_order_name=&onlygroups=0&s_available=&articlelist_type=search&s_volltext=3090",
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
            GetStock(Videocard.RTX3060TI, "RTX 3060", values);
            GetStock(Videocard.RTX3070, "RTX 3070", values);
            GetStock(Videocard.RTX3080, "RTX 3080", values);
            GetStock(Videocard.RTX3090, "RTX 3090", values);
            return new Stock(this, values);
        }

        #endregion

        #region Private

        /// <summary>
        /// Get the stock.
        /// </summary>
        /// <param name="card">The card<see cref="Videocard"/>.</param>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="values">The values<see cref="Dictionary{Videocard, int}"/>.</param>
        private void GetStock(Videocard card, string name, Dictionary<Videocard, int> values)
        {
            string html = WebsiteDownloader.GetHtml(GetProductUrl(card));

            try
            {
                html = html.Replace(@"\", string.Empty);
                var splittedHtml = html.Split("<div class=\"es_articellist_kachel-box\">");
                var filteredByName = splittedHtml.Where(o => o.Contains(name) && !o.Contains("DOCTYPE")).ToList();
                var filtered = filteredByName.Where(o => !o.Contains("nicht lieferbar")).ToList();
                values.Add(card, filtered.Count());
            }
            catch (Exception)
            { }
        }

        #endregion
    }
}
