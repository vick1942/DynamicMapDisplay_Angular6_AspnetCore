using Business.Entities;
using Business.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IRepository
{
    public interface IProviderRepository
    {
        Task<List<ProviderCategoryListEntity>> GetAllFacilityList(string collectionName, bool refresh);
        Task<List<SpecialityListEntity>> GetAllSpecialityList(string collectionName, bool refresh);
        Task<List<PEntity>> GetAllOrganizationDetails(bool refresh, string collectionName);
        SpecialityListEntity GetSpecialityById(string code);
        ProviderCategoryListEntity GetFacilityById(string code);
    }
}
