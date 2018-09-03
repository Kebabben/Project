using System;
using System.Linq;
using System.Web.Http;

/// <summary>
/// WebService API Controllers.
/// </summary>
namespace WebService.Controllers
{
    /// <summary>
    /// Controller for GETting weather. 
    /// </summary>
    [Route("api/weather")]    
    public class WeatherController : ApiController
    {
        /// <summary>
        /// GETter that provides a WeatherSheet at a specific longitude, latitude and the closest available time to input parameter. 
        /// </summary>
        /// <param name="lon">Longitude coordinate to get weather at.</param>
        /// <param name="lat">Latitude coordinate to get weather at.</param>
        /// <param name="dateTimeString">Date and time to get the forecast.</param>
        /// <returns></returns>
        [Route("api/weather/{lon}/{lat}/{dateTimeString}/")]
        [HttpGet]
        public IHttpActionResult GetWeather(string lon, string lat, string dateTimeString)
        {
            XmlManager.WeatherSheet weatherSheet = new XmlManager.WeatherSheet();
            DateTime dateTime = XmlManager.XmlManager.ConvertAPICompatiableDateTimeFormatToDateTime(dateTimeString);

            try
            {
                weatherSheet = SMHI.SMHI.GetWeatherFromSMHIByLonLatDateTime(lon, lat, dateTime);
            }
            catch (Exception ex)
            {
                weatherSheet.Error = true;
                weatherSheet.ExMsg = ex.Message;
            }

            return Ok(weatherSheet);
        }
    }

    /// <summary>
    /// Controllers for GETting information about locations and trips.
    /// </summary>
    [Route("api/")]
    public class TravelController : ApiController
    {
        /// <summary>
        /// GETter that provides a LocationListSheet with search results based upon input.
        /// </summary>
        /// <param name="id">The string to search for.</param>
        /// <returns></returns>
        [Route("api/destination/{id}")]
        [HttpGet]
        public IHttpActionResult GetDestinationName(string id)
        {            
            XmlManager.LocationSheetList locationSheetList = new XmlManager.LocationSheetList();

            try
            {
                locationSheetList = ResRobot.ResRobot.GetLocations(id);
            }
            catch(Exception ex)
            {
                locationSheetList.Error = true;
                locationSheetList.ExMsg = ex.Message;
            }
            
            return Ok(locationSheetList);                       
        }

        /// <summary>
        /// GETter for providing a TripSheet to input location from Örebro Centralstation.
        /// </summary>
        /// <param name="id">A resrobot API ID that corresponds with a location.</param>
        /// <returns></returns>
        [Route("api/trip/{id}")]
        [HttpGet]
        public IHttpActionResult GetTripToDestination(int id)
        {
            XmlManager.TripSheet tripSheet = new XmlManager.TripSheet();

            try
            {
                //Always travel from Örebro Centrailstation, ResRobot ID: 740000133                
                tripSheet = ResRobot.ResRobot.GetTripBetweenLocations(740000133, id);
            }
            catch(Exception ex)
            {
                tripSheet.Error = true;
                tripSheet.ExMsg = ex.Message;
            }

            return Ok(tripSheet);
        }
    }

    /// <summary>
    /// Controller for GETting a combined XML of weather and trip information.
    /// </summary>
    [Route("api/tripandweather")]
    public class TripToLocationController : ApiController
    {        
        /// <summary>
        /// Searches for a destination and returns information about departure, arrival and weather for the trip to that location.
        /// </summary>
        /// <param name="id">A string to search for a trip to.</param>
        /// <returns></returns>
        [Route("api/tripandweather/{id}")]
        [HttpGet]
        public IHttpActionResult GetDestinationName(string id)
        {
            XmlManager.TripAndWeatherSheet tripAndWeatherSheet = new XmlManager.TripAndWeatherSheet();

            try
            {
                XmlManager.LocationSheetList locationSheetList;
                locationSheetList = ResRobot.ResRobot.GetLocations(id);

                XmlManager.TripSheet tripSheet = new XmlManager.TripSheet();
                tripSheet = ResRobot.ResRobot.GetTripBetweenLocations(740000133, int.Parse(locationSheetList.LocationSheets.First().ID));

                XmlManager.WeatherSheet weatherSheet = SMHI.SMHI.GetWeatherFromSMHIByLonLatDateTime(tripSheet.DestinationLon, tripSheet.DestinationLat, 
                    DateTime.Parse(tripSheet.DestinationTime));

                tripAndWeatherSheet.tripSheet = tripSheet;
                tripAndWeatherSheet.weatherSheet = weatherSheet;
            }
            catch(Exception ex)
            {
                tripAndWeatherSheet.Error = true;
                tripAndWeatherSheet.ExMsg = ex.Message;
            }

            return Ok(tripAndWeatherSheet);
        }
    }
}
