using System;
using System.Collections.Generic;
using System.Globalization;

namespace XmlManager
{
    /// <summary>
    /// Information if WebService encountered a error while getting information from other API.
    /// </summary>
    public class ErrorMessage
    {
        public bool Error { get; set; } = false;
        public string ExMsg { get; set; } = "";
    }

    /// <summary>
    /// Class for storing information about weather.
    /// </summary>
    public class WeatherSheet : ErrorMessage
    {
        public string AirTemperature { get; set; }
        public string WindDirection { get; set; }
        public string WindSpeed { get; set; }
        public string MeanVAlueOfTotalCloudCover { get; set; }
        public string MinimumPrecipitationIntensity { get; set; }
        public string MaximumPrecipitationIntensity { get; set; }
        public string PrecipitationInFrozenForm { get; set; }
        public string MeanPrecipitationIntensity { get; set; }
        public string PrecipitationCategory { get; set; }
        public string WeatherSymbol { get; set; }
    }

    /// <summary>
    /// List of LocationSheet
    /// </summary>
    public class LocationSheetList : ErrorMessage
    {
        public List<LocationSheet> LocationSheets { get; set; }      
    }
     
    /// <summary>
    /// Information about locations.
    /// </summary>
    public class LocationSheet : ErrorMessage
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }

    /// <summary>
    /// Information about a trip between two locations.
    /// </summary>
    public class TripSheet : ErrorMessage
    {
        public string OriginName { get; set; }    
        public string OriginTime { get; set; }
        public string OriginDate { get; set; }
        public string OriginLon { get; set; }
        public string OriginLat { get; set; }
        public string DestinationName { get; set; }
        public string DestinationTime { get; set; }
        public string DestinationDate { get; set; }
        public string DestinationLon { get; set; }
        public string DestinationLat { get; set; }
    }

    /// <summary>
    /// Combined class of weather and trip information.
    /// </summary>
    public class TripAndWeatherSheet : ErrorMessage
    {
        public WeatherSheet weatherSheet { get; set; }
        public TripSheet tripSheet { get; set; }
    }

    /// <summary>
    /// Several functions vital to diffrent projects.
    /// </summary>
    public class XmlManager
    {
        /// <summary>
        /// Converts a regular DateTime to a string that can be passed as a attribute via API.
        /// </summary>
        /// <param name="dateTime">A DateTime object.</param>
        /// <returns></returns>
        public static string ConvertDateTimeToAPICompatiableDateTimeFormat(DateTime dateTime)
        {
            return dateTime.ToString("yyyyMMddHHmm");
        }

        /// <summary>
        /// Converts an API attribute compatiable DateTime string to regular DateTime format. 
        /// </summary>
        /// <param name="dateTime">A specially formated DateTime string.</param>
        /// <returns></returns>
        public static DateTime ConvertAPICompatiableDateTimeFormatToDateTime(string dateTime)
        {
            return DateTime.ParseExact(dateTime, "yyyyMMddHHmm", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Makes a WeatherSheet.
        /// </summary>
        /// <param name="airTemperature"></param>
        /// <param name="windDirection"></param>
        /// <param name="windSpeed"></param>
        /// <returns>A WeatherSheet</returns>
        public static WeatherSheet MakeWeatherSheet(string airTemperature, string windDirection, string windSpeed)
        {
            WeatherSheet weatherSheet = new WeatherSheet();

            weatherSheet.AirTemperature = airTemperature;
            weatherSheet.WindDirection = windDirection;
            weatherSheet.WindSpeed = windSpeed;
                        
            return weatherSheet;
        }

        /// <summary>
        /// Makes a LocationSheet
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <returns>A LocationSheet</returns>
        public static LocationSheet MakeLocationSheet(string id, string name, string longitude, string latitude)        
        {
            LocationSheet locationSheet = new LocationSheet();

            locationSheet.ID = id;
            locationSheet.Name = name;
            locationSheet.Longitude = longitude;
            locationSheet.Latitude = latitude;

            return locationSheet;
        }

        /// <summary>
        /// Makes a TripSheet.
        /// </summary>
        /// <param name="originName"></param>
        /// <param name="originTime"></param>
        /// <param name="originDate"></param>
        /// <param name="originLon"></param>
        /// <param name="originLat"></param>
        /// <param name="destinationName"></param>
        /// <param name="destinationTime"></param>
        /// <param name="destinationDate"></param>
        /// <param name="destinationLon"></param>
        /// <param name="destinationLat"></param>
        /// <returns>A TripSheet</returns>
        public static TripSheet MakeTripSheet(string originName, string originTime, string originDate, string originLon, string originLat,
            string destinationName, string destinationTime, string destinationDate, string destinationLon, string destinationLat)
        {
            TripSheet tripSheet = new TripSheet();
            
            tripSheet.OriginName = originName;
            tripSheet.OriginTime = originTime;
            tripSheet.OriginDate = originDate;
            tripSheet.OriginLon = originLon;
            tripSheet.OriginLat = originLat;

            tripSheet.DestinationName = destinationName;
            tripSheet.DestinationTime = destinationTime;
            tripSheet.DestinationDate = destinationDate;
            tripSheet.DestinationLon = destinationLon;
            tripSheet.DestinationLat = destinationLat;

            return tripSheet;
        }
    }
}