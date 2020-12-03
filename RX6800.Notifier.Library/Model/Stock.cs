using System;
using System.Collections.Generic;

namespace RX6800.Notifier.Library.Model
{
    /// <summary>
    /// Defines the <see cref="Stock" />.
    /// </summary>
    public class Stock
    {
        #region Constructor & Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Stock"/> class.
        /// </summary>
        /// <param name="website">The website<see cref="IWebsite"/>.</param>
        /// <param name="values">The values<see cref="Dictionary{Videocard, int}"/>.</param>
        public Stock(IWebsite website, Dictionary<Videocard, int> values)
        {
            this.Website = website;
            this.Timestamp = DateTime.Now;
            this.Values = values;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Website.
        /// </summary>
        public IWebsite Website { get; private set; }

        /// <summary>
        /// Gets the Timestamp.
        /// </summary>
        public DateTime Timestamp { get; private set; }

        /// <summary>
        /// Gets the Values.
        /// </summary>
        public Dictionary<Videocard, int> Values { get; private set; }

        #endregion
    }
}
