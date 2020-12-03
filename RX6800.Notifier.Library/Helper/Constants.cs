using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace RX6800.Notifier.Library.Helper
{
    /// <summary>
    /// Defines the <see cref="Constants" />.
    /// </summary>
    public static class Constants
    {
        #region Variables

        /// <summary>
        /// Defines the values.
        /// </summary>
        public static Dictionary<string, string> values = ReadJson();

        #endregion

        #region Public

        /// <summary>
        /// The MongoDB name.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public static string GetMongoDatabaseName()
        {
            if (values.ContainsKey("mongodbdatabasename"))
            {
                return values["mongodbdatabasename"];
            }
            else
            {
                Logger.JsonReadError("mongodbdatabasename");
                return "";
            }
        }

        /// <summary>
        /// The MongoDB connection string.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public static string GetMongoConnectionString()
        {
            if (values.ContainsKey("mongdbconnectionstring"))
            {
                return values["mongdbconnectionstring"];
            }
            else
            {
                Logger.JsonReadError("mongodbconnectionstring");
                return "";
            }
        }

        /// <summary>
        /// The reload interval.
        /// </summary>
        /// <returns>The <see cref="int"/>.</returns>
        public static int GetReloadInterval()
        {
            int retVal = -1;

            if (values.ContainsKey("reloadinterval"))
            {
                int.TryParse(values["reloadinterval"], out retVal);
            }

            if (retVal == -1)
            {
                Logger.JsonReadError("reloadinterval");
                retVal = 600000;
            }

            return retVal;
        }

        /// <summary>
        /// The email smtp host address.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public static string GetEmailHost()
        {
            if (values.ContainsKey("emailhost"))
            {
                return values["emailhost"];
            }
            else
            {
                Logger.JsonReadError("emailhost");
                return "";
            }
        }

        /// <summary>
        /// The email smtp host ip address.
        /// </summary>
        /// <returns>The <see cref="int"/>.</returns>
        public static int GetEmailPort()
        {
            int port = -1;

            if (values.ContainsKey("emailport"))
            {
                int.TryParse(values["emailport"], out port);
            }

            if (port == -1)
            {
                Logger.JsonReadError("reloadinterval");
                port = 587;
            }

            return port;
        }

        /// <summary>
        /// The email username.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public static string GetEmailUsername()
        {
            if (values.ContainsKey("emailusername"))
            {
                return values["emailusername"];
            }
            else
            {
                Logger.JsonReadError("emailusername");
                return "";
            }
        }

        /// <summary>
        /// The email password.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public static string GetEmailPassword()
        {
            if (values.ContainsKey("emailpassword"))
            {
                return values["emailpassword"];
            }
            else
            {
                Logger.JsonReadError("emailpassword");
                return "";
            }
        }

        /// <summary>
        /// The error log email address.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public static string GetErrorLogAddress()
        {
            if (values.ContainsKey("errorlogaddress"))
            {
                return values["errorlogaddress"];
            }
            else
            {
                Logger.JsonReadError("errorlogaddress");
                return "";
            }
        }

        /// <summary>
        /// Set the logging to verbose.
        /// </summary>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool GetVerboseMode()
        {
            bool ret = false;
            if (values.ContainsKey("verbosemode"))
            {
                Boolean.TryParse(values["verbosemode"], out ret);
            }
            return ret;
        }

        /// <summary>
        /// Use windows 10 toasts
        /// </summary>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool GetUseToasts()
        {
            bool ret = false;
            if (values.ContainsKey("usetoast"))
            {
                Boolean.TryParse(values["usetoast"], out ret);
            }
            return ret;
        }
        
        /// <summary>
        /// Use mongo db
        /// </summary>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool GetUseMongoDb()
        {
            bool ret = false;
            if (values.ContainsKey("useMongoDb"))
            {
                Boolean.TryParse(values["useMongoDb"], out ret);
            }
            return ret;
        }

        #endregion

        #region Private

        /// <summary>
        /// Read the constants.json.
        /// </summary>
        /// <returns>The <see cref="Dictionary{string, string}"/>.</returns>
        private static Dictionary<string, string> ReadJson()
        {
            try
            {
                using StreamReader file = new StreamReader("constants.json");
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(file.ReadToEnd());
            }
            catch (Exception)
            {
                throw new Exception("Error reading constants.json");
            }
        }

        #endregion
    }
}
