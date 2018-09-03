using System;
using System.Collections.Generic;
using System.Linq;

namespace SMHI
{
    /// <summary>
    /// Int values for easier acess of the values for the Parameters class from SMHI API.
    /// </summary>
    public static class SMHIWeatherParameter
    {
        public const int AirPressure = 0;
        public const int AirTemperature = 11;
        public const int HorizontalVisibility = 2;
        public const int WindDirection = 3;
        public const int WindSpeed = 14;
        public const int RelativeHumidity = 5;
        public const int ThunderProbability = 6;
        public const int MeanValueOfTotalCloudCover = 7;
        public const int MeanValueOfLowLevelCloudCover = 8;
        public const int MeanValueOfMediumLevelCloudCover = 9;
        public const int MeanValueOfHighLevelCloudCover = 10;
        public const int WindGustSpeed = 11;
        public const int MinimumPrecipitationIntensity = 12;
        public const int MaximumPrecipitationIntensity = 13;
        public const int PercentOfPrecipitationInFrozenForm = 14;
        public const int PrecipitationCategory = 15;
        public const int MeanPrecipitationIntensity = 16;
        public const int MedianPrecipitationIntensity = 17;
        public const int WeatherSymbol = 18;
    }

    /// <summary>
    /// Outermost class for easier parsing of data from SMHI API.
    /// </summary>
    public struct Information
    {
        public DateTime approvedTime { get; set; }
        public DateTime referenceTime { get; set; }
        public Geometry geometry { get; set; }
        public List<TimeSeries> timeSeries { get; set; }
    }

    /// <summary>
    /// Class for easier parsing of data from SMHI API.
    /// </summary>
    public struct Geometry
    {
        public String type { get; set; }
        public List<List<Double>> coordinates { get; set; }
    }
   
    /// <summary>
    /// Class for easier parson of data from SMHI API. Contains information about at what time a weather forecast is valid and holds a list of the values for the forecast.
    /// </summary>
    public struct TimeSeries
    {
        public DateTime validTime { get; set; }
        public List<Parameters> parameters { get; set; }
    }

    /// <summary>
    /// Class for easier parsing of data from SMHI API. Contains infomration about the weather. 
    /// </summary>
    public struct Parameters
    {
        public String name { get; set; }
        public String levelType { get; set; }
        public String level { get; set; }
        public String unit { get; set; }
        public List<String> values { get; set; }       
    }
    
    public class SMHI
    {
        /// <summary>
        /// Gets WeatherSheet data from SMHI API based upon long and lat coordinate.
        /// </summary>
        /// <param name="lon">Longitude coordinate.</param>
        /// <param name="lat">Latitude coodinate</param>
        /// <param name="dateTime">Specified time to get the closest forecast to</param>
        /// <returns></returns>
        public static XmlManager.WeatherSheet GetWeatherFromSMHIByLonLatDateTime(string lon, string lat, DateTime dateTime)
        {
            String JSonSMHIData = new System.Net.WebClient().DownloadString(
                "https://opendata-download-metfcst.smhi.se/api/category/pmp3g/version/2/geotype/point/lon/" + lon + "/lat/" + lat + "/data.json");

            Information information = Newtonsoft.Json.JsonConvert.DeserializeObject<Information>(JSonSMHIData);

            XmlManager.WeatherSheet weatherSheet = GetWeatherWithClosestTime(information, dateTime);

            return weatherSheet;
        }

        /// <summary>
        /// Returns a WeatherSheet object that is the closest forecast to dateTime parameter.
        /// </summary>
        /// <param name="info">Class of type Information from SMHI API.</param>
        /// <param name="dateTime">Specified time to get the closest forecast to.</param>
        /// <returns></returns>
        public static XmlManager.WeatherSheet GetWeatherWithClosestTime(Information info, DateTime dateTime)
        {
            TimeSeries SelectedTimeSeries = info.timeSeries.First();
            foreach(TimeSeries timeSeries in info.timeSeries)
            {
                if ((timeSeries.validTime - dateTime).Duration() < (SelectedTimeSeries.validTime - dateTime).Duration())
                {
                    SelectedTimeSeries = timeSeries;
                }
            }
            XmlManager.WeatherSheet weatherSheet;

            weatherSheet = XmlManager.XmlManager.MakeWeatherSheet(SelectedTimeSeries.parameters[SMHIWeatherParameter.AirTemperature].values.First(),
                SelectedTimeSeries.parameters[SMHIWeatherParameter.WindDirection].values.First(), SelectedTimeSeries.parameters[SMHIWeatherParameter.WindSpeed].values.First());

            return weatherSheet;
        }       
        
        static void Main(string[] args)
        {           
            Console.ReadKey();
        }
    }
}
