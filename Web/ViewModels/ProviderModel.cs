using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels
{
    public class ProviderModel
    {
        public string ProviderFirstName { get; set; }
        public string ProviderLastName { get; set; }
        public string ProviderFullName { get; set; }
        public string ServiceAddress1 { get; set; }
        public string ServiceAddress2 { get; set; }
        public string ServiceCity { get; set; }
        public string ServiceState { get; set; }
        public string ServiceZip { get; set; }
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string LocationPracticeAddress { get; set; }
        public string Specialization { get; set; }
        public string Facility { get; set; }
        public string RadiusDistance { get; set; }
        public string PhoneAreaCode { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneExtension { get; set; }
        public string ProviderType { get; set; }
        public string ProviderCategory { get; set; }
    }
}
