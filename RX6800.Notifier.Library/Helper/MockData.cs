using RX6800.Notifier.Library.Model;
using System;
using System.Collections.Generic;

namespace RX6800.Notifier.Library.Helper
{
    /// <summary>
    /// Defines the <see cref="MockData" />.
    /// </summary>
    public static class MockData
    {
        #region Public

        /// <summary>
        /// Return empty stock values.
        /// </summary>
        /// <param name="website">The website<see cref="IWebsite"/>.</param>
        /// <returns>The <see cref="Stock"/>.</returns>
        public static Stock GetEmptyStock(IWebsite website)
        {
            Dictionary<Videocard, int> values = new Dictionary<Videocard, int>();

            foreach (Videocard card in Enum.GetValues(typeof(Videocard)))
            {
                values[card] = 0;
            }

            return new Stock(website, values);
        }

        #endregion
    }
}
