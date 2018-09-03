using System.Collections.Generic;
using System.Xml.Serialization;

namespace ResRobot
{
    /// <summary>
    /// Namespace for storing information from resrobot API for easier parsing.
    /// </summary>
    namespace XmlLocationsClass
    {
        
        [XmlRoot(ElementName = "StopLocation", Namespace = "hafas_rest_v1")]
        public class StopLocation
        {
            [XmlAttribute(AttributeName = "id")]
            public string Id { get; set; }
            [XmlAttribute(AttributeName = "extId")]
            public string ExtId { get; set; }
            [XmlAttribute(AttributeName = "name")]
            public string Name { get; set; }
            [XmlAttribute(AttributeName = "lon")]
            public string Lon { get; set; }
            [XmlAttribute(AttributeName = "lat")]
            public string Lat { get; set; }
            [XmlAttribute(AttributeName = "weight")]
            public string Weight { get; set; }
            [XmlAttribute(AttributeName = "products")]
            public string Products { get; set; }
        }

        [XmlRoot(ElementName = "LocationList", Namespace = "hafas_rest_v1")]
        public class LocationList
        {
            [XmlElement(ElementName = "StopLocation", Namespace = "hafas_rest_v1")]
            public List<StopLocation> StopLocation { get; set; }
            [XmlAttribute(AttributeName = "xmlns")]
            public string Xmlns { get; set; }
        }
    }

    
}
