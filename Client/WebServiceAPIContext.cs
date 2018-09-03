using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System;

namespace Client
{
    /// <summary>
    /// Used by the client for interacting with the WebService API.
    /// </summary>
    class WebServiceAPIContext
    {        
        /// <summary>
        /// GETs weather information from the WebService API.
        /// </summary>
        /// <param name="lat">Latitude coordinates.</param>
        /// <param name="lon">Longitude coordinates.</param>
        /// <param name="dateTime">Gets the weather at specified time.</param>
        /// <param name="connectionString">WebService API URL.</param>
        /// <returns>Returns a WeatherSheet with weather information in it.</returns>
        public static XmlManager.WeatherSheet ReceiveWeather(string lat, string lon, DateTime dateTime, string connectionString)
        {
            string StringDateTime = XmlManager.XmlManager.ConvertDateTimeToAPICompatiableDateTimeFormat(dateTime);

            string data = new WebClient().DownloadString(connectionString + "/api/weather/" + lon + "/" + lat + "/" + StringDateTime + "/");
            XmlManager.WeatherSheet xmlSheet = JsonConvert.DeserializeObject<XmlManager.WeatherSheet>(data);

            return xmlSheet;
        }

        /// <summary>
        /// GETs information from WebService API about locations that matches input parameter.
        /// </summary>
        /// <param name="id">String to search for.</param>
        /// <param name="connectionString">WebService API URL.</param>
        /// <returns>A LocationSheetList with locations that matches the id parameter.</returns>
        public static XmlManager.LocationSheetList ReceiveLocationList(string id, string connectionString)
        {
            XmlManager.LocationSheetList xmlSheet = new XmlManager.LocationSheetList();
            xmlSheet.LocationSheets = new List<XmlManager.LocationSheet>();

            WebClient webClient = new WebClient();

            //Need to set encoding to UTF8, else the special characters in some city names will look weird.
            webClient.Encoding = Encoding.UTF8;

            string data = webClient.DownloadString(connectionString + "/api/destination/" + id);


            xmlSheet = JsonConvert.DeserializeObject<XmlManager.LocationSheetList>(data);
            
            return xmlSheet;
        }

        /// <summary>
        /// GETs a trip from Örebro Centrailstation to destination ID. ID must correspond with the ID for a location from resrobot API.
        /// </summary>
        /// <param name="id">A resrobot ID for a location.</param>
        /// <param name="connectionString">WebService API URL.</param>
        /// <returns>Returns information about a trip from Örebro Centralstation.</returns>
        public static XmlManager.TripSheet ReceiveTripToDestination(int id, string connectionString)
        {
            string data = new WebClient().DownloadString(connectionString + "/api/trip/" + id);
            XmlManager.TripSheet xmlSheet = JsonConvert.DeserializeObject<XmlManager.TripSheet>(data);

            return xmlSheet;
        }
    }
}
