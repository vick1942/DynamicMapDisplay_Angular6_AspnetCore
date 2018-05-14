using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Entities
{
    public class ConfirmationEntity
    {
        public string GroupId { get; set; }
        public string OrganizationId { get; set; }
        public string GroupName { get; set; }
        public string GroupNumber { get; set; }
        public string PlanName { get; set; }
        public string NetworkCode { get; set; }
        public int Option { get; set; }
    }
}
