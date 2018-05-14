using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels
{
    public class OrganizationCollectionModel
    {
        public string _id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
        public List<Provider> Providers { get; set; }
        public List<Network> Networks { get; set; }
    }
    public class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
    }

    public class Taxonomy
    {
        public string TaxonomyCode { get; set; }
        public string LicenseNumber { get; set; }
        public string StateCode { get; set; }
        public string TaxonomySwitch { get; set; }
        public string Grouping { get; set; }
        public string Classification { get; set; }
        public string Specialization { get; set; }
    }

    public class OtherIdentifier
    {
        public string Identifier { get; set; }
        public string TypeCode { get; set; }
        public string State { get; set; }
        public string Issuer { get; set; }
    }

    public class Provider
    {
        public int _id { get; set; }
        public int EntityTypeCode { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string NamePrefix { get; set; }
        public string Credential { get; set; }
        public string MailingAddress1 { get; set; }
        public string MailingCity { get; set; }
        public string MailingState { get; set; }
        public string MailingZip { get; set; }
        public string MailingCountryCode { get; set; }
        public string MailingPhone { get; set; }
        public string MailingFax { get; set; }
        public string PracticeAddress1 { get; set; }
        public string PracticeCity { get; set; }
        public string PracticeState { get; set; }
        public string PracticeZip { get; set; }
        public string PracticeCountryCode { get; set; }
        public string PracticePhone { get; set; }
        public DateTime EnumerationDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string GenderCode { get; set; }
        public List<Taxonomy> Taxonomies { get; set; }
        public List<OtherIdentifier> OtherIdentifiers { get; set; }
        public string IsSoleProprietor { get; set; }
    }

    public class Logo
    {
    }

    public class Network
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string LookupURL { get; set; }
        public bool IsManaged { get; set; }
        public Logo Logo { get; set; }
    }
}
