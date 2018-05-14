using Business.Entities.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IBusiness
{
    public interface IImportService
    {
        Task<bool> IsOrganizationCodeExists(string organizationCode);
        Task<string> GetNetworkNameByCode(string networkCode, string organizationCode);
        Task<List<ProviderCategoryEntity>> GetAllProviderCategories();
        Task<List<SpecialityEntity>> GetAllSpecialities();
        Task SaveProviderCollection(List<PEntity> providers, string networkName, string fileName, int bulkInsertRecordsCount);
        Task UpdateSystemStatus();
        
    }   
}
