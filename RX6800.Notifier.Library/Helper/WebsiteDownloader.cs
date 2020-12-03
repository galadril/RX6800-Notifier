using System;
using System.IO;
using System.Net;

namespace RX6800.Notifier.Library.Helper
{
    /// <summary>
    /// Defines the <see cref="WebsiteDownloader" />.
    /// </summary>
    public static class WebsiteDownloader
    {
        #region Public

        /// <summary>
        /// Download the html content.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string GetHtml(string url)
        {
            try
            {
                WebClient client = new WebClient();

                client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

                Stream data = client.OpenRead(url);
                StreamReader reader = new StreamReader(data);
                string s = reader.ReadToEnd();

                data.Close();
                reader.Close();

                return s;
            }
            catch (Exception)
            {
                //Logger.HtmlDownloadError(url);
                return "";
            }
        }

        #endregion
    }
}
