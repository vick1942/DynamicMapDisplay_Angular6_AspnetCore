using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Web
{
    public class GroupModel
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
