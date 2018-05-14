using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Business.Entities.Entities
{
    public class InboundFile
    {
        public string OrganizationId { get; set; }
        public string OrganizationCode { get; set; }
        public string NetworkCode { get; set; }
        public string NetworkName { get; set; }

        public string TradingPartnerNumber { get; set; }

        public string GroupNumber { get; set; }

        public int? TradingPartnerId { get; set; }

        public int? GroupId { get; set; }

        public int? EligibilityInboundFileId { get; set; }

        public MemoryStream InboundStream { get; set; }

        public FileInfo InboundFileInfo { get; set; }

        public bool IsDuplicate { get; set; }

        public bool IsGroupActive { get; set; }

        public bool IsTradingPartnerActive { get; set; }

        public DateTime? DuplicateFileCatalogDate { get; set; }

    }
}
