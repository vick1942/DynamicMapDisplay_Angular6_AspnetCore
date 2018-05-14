using Business.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace IRepository
{
    public interface IImportRepository
    {
        Task SaveProviderCollection(List<PEntity> providers, string networkName, string fileName, int bulkInsertRecordsCount);
        Task<List<ProviderCategoryEntity>> GetAllProviderCategories();
        Task<List<SpecialityEntity>> GetAllSpecialities();
        Task<bool> IsOrganizationCodeExists(string organizationCode);
        Task<string> GetNetworkNameByCode(string networkCode ,string organizationCode);
        Task UpdateSystemStatus();
    }
}
