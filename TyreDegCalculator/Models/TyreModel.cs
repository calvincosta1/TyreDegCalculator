using System.Collections.Generic;
using System.Xml.Serialization;

namespace TyreDegCalculator.Models
{
    public class TyreModel
    {
        public class Tyre
        {
            // Properties of the tyre
            public string Name;
            public string Family;
            public string Type;
            public string Placement;
            public int DegradationCoefficient;
            public string Name2 => Name;
        }

        public class Tyres
        {
            [XmlElement("Tyre")]
            public List<Tyre> Tyress;
        }
    }
}