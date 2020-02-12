using System;
using System.Xml.Serialization;
using System.Collections.Generic;
namespace Models
{
    [XmlRoot(ElementName = "Zip")]
    public class Zip
    {
        [XmlAttribute(AttributeName = "code")]
        public string Code { get; set; }
    }

    [XmlRoot(ElementName = "District")]
    public class District
    {
        [XmlElement(ElementName = "Zip")]
        public List<Zip> Zip { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
    }

    [XmlRoot(ElementName = "City")]
    public class City
    {
        [XmlElement(ElementName = "District")]
        public List<District> District { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "code")]
        public string Code { get; set; }
    }

    [XmlRoot(ElementName = "AddressInfo")]
    public class AddressInfo
    {
        [XmlElement(ElementName = "City")]
        public List<City> City { get; set; }
    }

}
