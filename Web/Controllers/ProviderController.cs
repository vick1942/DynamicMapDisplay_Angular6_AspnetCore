using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IBusiness;
using Business.Entities;
using Common.Mappers;
using Web.ViewModels;
using Business.Entities.Entities;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    public class ProviderController : Controller
    {
        public readonly IProviderService _planService;
        public ProviderController(IProviderService planService)
        {
            _planService = planService;
        }
        #region Controllers
        [HttpGet("GetPlanDetails")]
        public async Task<IEnumerable<GroupModel>> GetPlanDetailsByName(string name)
        {
            List<ConfirmationEntity> groupEntity = await _planService.GetPlanDetailsByName(name);
            return getGroupList(groupEntity);
        }
        [HttpGet("GetGroupDetails")]
        public async Task<IEnumerable<GroupModel>> GetGroupDetailsByNameOrNumber(string nameOrNumber)
        {
            List<ConfirmationEntity> groupEntity = await _planService.GetGroupDetailsByNameOrNumber(nameOrNumber);
            return getGroupList(groupEntity);
        }

        [HttpGet("GetAllOrganizationDetails")]
        public async Task<IEnumerable<ProviderModel>> GetAllOrganizationDetails(int pageNumber, string networkCode)
        {
            List<ProviderResultDetails> groupAllList = await _planService.GetAllOrganizationDetails(pageNumber, networkCode);
            if (groupAllList != null)
            {
                IEnumerable<ProviderModel> groupModelList = ProviderEntityMapper<ProviderResultDetails, ProviderModel>.MapEntityCollection(groupAllList);
                return groupModelList;
            }
            return null;
        }
        #endregion

        [HttpGet("GetAllSpecialityList")]
        public async Task<IEnumerable<SpecialityListEntity>> GetAllSpecialityList(int pageNumber, string collectionName, bool refresh)
        {
            List<SpecialityListEntity> specialityList = await _planService.GetAllSpecialityList(pageNumber,collectionName);
            return specialityList;
        }

        [HttpGet("GetAllFacilityList")]
        public async Task<IEnumerable<ProviderCategoryListEntity>> GetAllFacilityList(int pageNumber, string collectionName)
        {
            List<ProviderCategoryListEntity> facilityList = await _planService.GetAllFacilityList(pageNumber,collectionName);
            return facilityList;
        }

        [HttpGet("GetFilteredOrganizationDetails")]
        public async Task<IEnumerable<ProviderModel>> GetFilteredOrganizationDetails(string networkCode, string zipCode, int miles, int pageNumber, string specialization, string facility, string providerOrFacilityName, int providerId)
        {
            List<ProviderResultDetails> providerList = await _planService.GetFilteredOrganizationDetails(networkCode, zipCode, miles, pageNumber, specialization, facility, providerOrFacilityName, providerId);
            IEnumerable<ProviderModel> providerModelList = ProviderEntityMapper<ProviderResultDetails, ProviderModel>.MapEntityCollection(providerList);
            return providerModelList;
        }

        #region Methods
        private IEnumerable<GroupModel> getGroupList(List<ConfirmationEntity> groupEntityList)
        {
            IEnumerable<GroupModel> groupModelList = ProviderEntityMapper<ConfirmationEntity, GroupModel>.MapEntityCollection(groupEntityList);
            return groupModelList;
        }
        #endregion
    }
}