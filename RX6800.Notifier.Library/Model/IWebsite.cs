namespace RX6800.Notifier.Library.Model
{
    /// <summary>
    /// Defines the <see cref="IWebsite" />.
    /// </summary>
    public interface IWebsite
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Url.
        /// </summary>
        public string Url { get; set; }

        #endregion

        #region Public

        /// <summary>
        /// Get the stock values.
        /// </summary>
        /// <returns>The <see cref="Stock"/>.</returns>
        public Stock GetStock();

        /// <summary>
        /// Get the product releated direct url.
        /// </summary>
        /// <param name="card">The card<see cref="Videocard"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetProductUrl(Videocard card);

        #endregion
    }
}
