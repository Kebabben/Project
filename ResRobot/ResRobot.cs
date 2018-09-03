using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace ResRobot
{    
    public class ResRobot
    {
        /// <summary>
        /// Searches for city names and returns LocationSheetList with results. Default is 10 returned results.
        /// </summary>
        /// <param name="name">Search string</param>
        /// <param name="amount">Amount of results to return</param>
        /// <returns></returns>
        public static XmlManager.LocationSheetList GetLocations(String name, int amount = 10)
        {
            XmlReader Reader = XmlReader.Create
                ("https://api.resrobot.se/v2/location.name?key=0e8eb279-dd0e-4b30-8f5d-ca0e244f9b6b&maxNo=" + amount + "&input=" + name + "?");
        
            XmlSerializer seri = new XmlSerializer(typeof(XmlLocationsClass.LocationList));
            XmlLocationsClass.LocationList locationList = (XmlLocationsClass.LocationList)seri.Deserialize(Reader);


            XmlManager.LocationSheetList locationSheetList = new XmlManager.LocationSheetList();
            locationSheetList.LocationSheets = new List<XmlManager.LocationSheet>();

            foreach(XmlLocationsClass.StopLocation stopLocation in locationList.StopLocation)
            {
                locationSheetList.LocationSheets.Add(XmlManager.XmlManager.MakeLocationSheet(stopLocation.Id, stopLocation.Name, stopLocation.Lon, stopLocation.Lat));
            }

            return locationSheetList;
        }

        /// <summary>
        /// Returns a TripSheet with information about a trip between two locations.
        /// </summary>
        /// <param name="id1">ID of location 1</param>
        /// <param name="id2">ID of location 2</param>
        /// <returns></returns>
        public static XmlManager.TripSheet GetTripBetweenLocations(int id1, int id2)
        {
            XmlReader Reader = XmlReader.Create
                ("https://api.resrobot.se/v2/trip?key=0e8eb279-dd0e-4b30-8f5d-ca0e244f9b6b&originId=" + id1 + "&destId=" + id2 + "&format=xml");

            XmlSerializer seri = new XmlSerializer(typeof(XmlTripsClass.TripList));

            XmlTripsClass.TripList tripList = (XmlTripsClass.TripList)seri.Deserialize(Reader);
            
            XmlManager.TripSheet tripSheet = new XmlManager.TripSheet();
            
            tripSheet = XmlManager.XmlManager.MakeTripSheet(tripList.Trip.First().LegList.Leg.First().Origin.Name,
                tripList.Trip.First().LegList.Leg.First().Origin.Time,
                tripList.Trip.First().LegList.Leg.First().Origin.Date,
                tripList.Trip.First().LegList.Leg.First().Origin.Lon,
                tripList.Trip.First().LegList.Leg.First().Origin.Lat,
                tripList.Trip.First().LegList.Leg.Last().Destination.Name,
                tripList.Trip.First().LegList.Leg.Last().Destination.Time,
                tripList.Trip.First().LegList.Leg.Last().Destination.Date,
                tripList.Trip.First().LegList.Leg.Last().Destination.Lon,
                tripList.Trip.First().LegList.Leg.Last().Destination.Lat);
            
            return tripSheet;
        }
        
        static void Main(string[] args)
        {            
            Console.ReadKey();
        }
    }
}
