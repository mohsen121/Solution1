using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DistanceApi.Domain
{
    [ComplexType]
    public class Location
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
