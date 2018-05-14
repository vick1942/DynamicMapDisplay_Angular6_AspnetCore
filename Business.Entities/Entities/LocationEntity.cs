using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Entities
{
    public class LocationEntity
    {
        public int Id { get; set; }
        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Address { get; set; }

        public string ErrorMessage_GoogleMapAPI { get; set; }
        public string Status_GoogleMapAPI { get; set; }
    }
}
