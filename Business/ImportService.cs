using Business.Entities;
using Business.Entities.Entities;
using Common.Constants;
using Common.Utilities;
using IBusiness;
using IRepository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class ImportService : IImportService
    {
        private readonly IImportRepository _importRepository;
        private List<ProviderCategoryEntity> providerCategories;
        private List<SpecialityEntity> specialities;

        public ImportService(IImportRepository importRepository)
        {
            _importRepository = importRepository;
        }

        public async Task<bool> IsOrganizationCodeExists(string organizationCode)
        {
            return await _importRepository.IsOrganizationCodeExists(organizationCode).ConfigureAwait(false);
        }

        public async Task<string> GetNetworkNameByCode(string networkCode, string organizationCode)
        {
            return await _importRepository.GetNetworkNameByCode(networkCode, organizationCode).ConfigureAwait(false);
        }

        public async Task<List<ProviderCategoryEntity>> GetAllProviderCategories()
        {
            return await _importRepository.GetAllProviderCategories().ConfigureAwait(false);
        }

        public async Task<List<SpecialityEntity>> GetAllSpecialities()
        {
            return await _importRepository.GetAllSpecialities().ConfigureAwait(false);
        }

        public async Task SaveProviderCollection(List<PEntity> providers, string networkName, string fileName, int bulkInsertRecordsCount)
        {
             await _importRepository.SaveProviderCollection(providers, networkName, fileName, bulkInsertRecordsCount);
        }
        public async Task UpdateSystemStatus()
        {
            await _importRepository.UpdateSystemStatus();
        }
    }
}
