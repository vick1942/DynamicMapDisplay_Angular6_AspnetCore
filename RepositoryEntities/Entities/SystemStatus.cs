using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Entities.Entities
{
    public class SystemStatus
    {
        public string SystemName { get; set; }
        public DateTime LastPingDate { get; set; }
    }
}
