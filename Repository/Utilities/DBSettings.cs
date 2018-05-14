using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Utilities
{
    public class DbSettings
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
        public string GroupCollection { get; set; }
        public string PlanCollection { get; set; }
        public string OrganizationCollection { get; set; }
        public string EliteCollection { get; set; }
        public string SpecialityCollection { get; set; }
        public string ProviderCategoryCollection { get; set; }
        public string SystemStatusCollection { get; set; }

    }
}
