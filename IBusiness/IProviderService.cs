using Business.Entities;
using Business.Entities.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IBusiness
{
    public interface IProviderService
    {
        Task<List<ConfirmationEntity>> GetPlanDetailsByName(string name);
        Task<List<ConfirmationEntity>> GetGroupDetailsByNameOrNumber(string nameOrNumber);
        Task<List<PlanEntity>> GetAllPlanDetails();
        Task<List<GroupEntity>> GetAllGroupDetails();
        Task<List<ProviderResultDetails>> GetAllOrganizationDetails(int pageNumber, string networkCode);
        Task<List<ProviderResultDetails>> GetFilteredOrganizationDetails(string networkCode, string zipCode, int miles, int pageNumber, string specialization, string facility, string providerOrFacilityName, int providerId);
        Task<List<SpecialityListEntity>> GetAllSpecialityList(int pageNumber, string collectionName);
        Task<List<ProviderCategoryListEntity>> GetAllFacilityList(int pageNumber, string collectionName);
    }
}
