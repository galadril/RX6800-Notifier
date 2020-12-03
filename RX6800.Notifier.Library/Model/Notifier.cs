using RX6800.Notifier.Library.Helper;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Timers;

namespace RX6800.Notifier.Library.Model
{
    /// <summary>
    /// Defines the <see cref="Notifier" />.
    /// </summary>
    public class Notifier
    {
        #region Variables

        /// <summary>
        /// Defines the websites.
        /// </summary>
        private List<IWebsite> websites;

        //stores the stock information of the previous fetch.
        /// <summary>
        /// Defines the stockRecords.
        /// </summary>
        private Dictionary<IWebsite, Stock> stockRecords;

        /// <summary>
        /// Defines the timer.
        /// </summary>
        private readonly System.Timers.Timer timer;

        /// <summary>
        /// Defines the verboseMode.
        /// </summary>
        private bool verboseMode;

        #endregion

        #region Constructor & Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Notifier"/> class.
        /// </summary>
        public Notifier()
        {
            this.verboseMode = Constants.GetVerboseMode();
            this.websites = new List<IWebsite>();
            this.stockRecords = new Dictionary<IWebsite, Stock>();

            this.timer = new System.Timers.Timer(Constants.GetReloadInterval());
            this.timer.Elapsed += TimerElapsed;
        }

        #endregion

        #region Public

        /// <summary>
        /// The TrackWebsite.
        /// </summary>
        /// <param name="website">The website<see cref="IWebsite"/>.</param>
        public void TrackWebsite(IWebsite website)
        {
            this.websites.Add(website);
            this.stockRecords.Add(website, null);
        }

        /// <summary>
        /// The Start.
        /// </summary>
        public void Start()
        {
            this.GetStocksThreaded();
            this.timer.Start();
        }

        #endregion

        #region Private

        /// <summary>
        /// The TimerElapsed.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="ElapsedEventArgs"/>.</param>
        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            GetStocksThreaded();
        }

        /// <summary>
        /// The GetStocksThreaded.
        /// </summary>
        private void GetStocksThreaded()
        {
            new Thread(GetStocks).Start();
        }

        /// <summary>
        /// The GetStocks.
        /// </summary>
        private void GetStocks()
        {
            foreach (IWebsite website in this.websites)
            {
                new Thread(() => GetStock(website)).Start();
            }
        }

        /// <summary>
        /// The GetStock.
        /// </summary>
        /// <param name="website">The website<see cref="IWebsite"/>.</param>
        private void GetStock(IWebsite website)
        {
            Stock stock = website.GetStock();
            if (CheckStockChange(website, stock) && Constants.GetUseMongoDb())
                Mongo.InsertStock(stock);

            if (this.verboseMode)
                Printer.PrintStock(stock);

            this.stockRecords[website] = stock;
        }

        /// <summary>
        /// The CheckStockChange.
        /// </summary>
        /// <param name="website">The website<see cref="IWebsite"/>.</param>
        /// <param name="stock">The stock<see cref="Stock"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        private bool CheckStockChange(IWebsite website, Stock stock)
        {
            bool ret = false;
            foreach (Videocard videocard in Enum.GetValues(typeof(Videocard)))
            {
                if (this.stockRecords[website] != null && this.stockRecords[website].Values[videocard] < stock.Values[videocard])
                {
                    Mailer.ProductUrl = stock.Website.GetProductUrl(videocard);
                    Mailer.SendToast(stock.Website.GetType().Name, stock.Website.GetType().Name + " has a " + Enum.GetName(typeof(Videocard), videocard));
                    Mailer.SendNotificationsThreaded(stock, videocard);
                    Logger.StockUpdate(stock, videocard);
                    ret = true;
                }
            }
            return ret;
        }

        #endregion
    }
}
