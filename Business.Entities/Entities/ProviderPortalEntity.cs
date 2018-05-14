using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Entities.Entities
{
    public class PEntity
    {
        public int IdGeneratedNumber { get; set; }
        public string ProviderTIN { get; set; }
        public string ProviderFullName { get; set; }
        public string BillingAddress1 { get; set; }
        public string BillingAddress2 { get; set; }
        public string BillingCity { get; set; }
        public string BillingState { get; set; }
        public string BillingZip { get; set; }
        public string PhoneAreaCode { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneExtension { get; set; }
        public string ProviderSpecialityCode { get; set; }
        public string ServiceAddress1 { get; set; }
        public string ServiceAddress2 { get; set; }
        public string ServiceCity { get; set; }
        public string ServiceState { get; set; }
        public string ServiceZip { get; set; }
        public string ProviderCategory { get; set; }
        public string ProviderFirstName { get; set; }
        public string ProviderLastName { get; set; }
        public string ProviderNPI { get; set; }
        public string ProviderFeeScheduleIdentifier { get; set; }
        public string ProviderGroupAssignment { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public double? DiscountPercentage { get; set; }
        public string Endorsedornonendorsed { get; set; }
        public string SecondProviderSpecialtyifapplicable { get; set; }
        public string ContractName { get; set; }
        public string EPOidentifier { get; set; }
        public string MiddleInitialIfApplicable { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }

}
