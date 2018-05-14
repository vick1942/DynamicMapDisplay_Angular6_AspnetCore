using System;
using Business.Entities;
using IBusiness;
using IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Entities.Entities;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using GeoCoordinatePortable;
using Common.Utilities;
using Common.Constants;
using Microsoft.Extensions.Options;
using NLog;

namespace Business
{
    public class ProviderService : IProviderService
    {
        private readonly IProviderRepository _providerRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IGroupRepository _groupRepository;
        private static List<ProviderResultDetails> providersCacheWithLatLong = new List<ProviderResultDetails>();
        private readonly IOptions<ConfigurationKeys> _configurationKeys = null;
        Logger loggers = LogManager.GetLogger(Constants.PWeb);
        public ProviderService(IProviderRepository providerRepository, IOrganizationRepository organizationRepository, IGroupRepository groupRepository, IOptions<ConfigurationKeys> configurationKeys)
        {
            _providerRepository = providerRepository;
            _organizationRepository = organizationRepository;
            _groupRepository = groupRepository;
            _configurationKeys = configurationKeys;
        }
        public async Task<List<ConfirmationEntity>> GetPlanDetailsByName(string name)
        {
            try
            {
                List<GroupEntity> groupEntityList = await _groupRepository.GetPlanDetailsByName(name);
                return PrepareGroupEntity(groupEntityList);
            }
            catch (Exception ex)
            {
                loggers.Error($"{Constants.PWeb} failed to get plan details by name.");
                return null;
            }
        }
        public async Task<List<ConfirmationEntity>> GetGroupDetailsByNameOrNumber(string nameOrNumber)
        {
            try
            {
                List<GroupEntity> groupEntityList = await _groupRepository.GetGroupDetailsByNameOrNumber(nameOrNumber);
                return PrepareGroupEntity(groupEntityList);
            }
            catch (Exception ex)
            {
                loggers.Error($"{Constants.PWeb} failed to get group details by name or number{nameOrNumber}.");
                return null;
            }
        }
        public async Task<List<PlanEntity>> GetAllPlanDetails()
        {
            try
            {
                return await _groupRepository.GetAllPlanDetails();
            }
            catch (Exception ex)
            {
                loggers.Error($"{Constants.PWeb} failed to get all plan details.");
                return null;
            }
        }
        public async Task<List<GroupEntity>> GetAllGroupDetails()
        {
            try
            {
                return await _groupRepository.GetAllGroupDetails();
            }
            catch (Exception ex)
            {
                loggers.Error($"{Constants.PWeb} failed to get all group details.");
                return null;
            }
        }
        public async Task<List<ProviderResultDetails>> GetAllOrganizationDetails(int pageNumber, string networkCode)
        {
            try
            {
                var refreshData = pageNumber == 1 ? true : false;
                string collectionName = await _organizationRepository.GetCollectionName(networkCode);
                if (collectionName != null)
                {
                    List<PEntity> providerList = await _providerRepository.GetAllOrganizationDetails(refreshData, collectionName);
                    if (providerList != null)
                    {
                        List<ProviderResultDetails> ProviderTopSliceList = (from pl in providerList
                                                                            select (new ProviderResultDetails
                                                                            {
                                                                                Id = pl.IdGeneratedNumber,
                                                                                ProviderFirstName = pl.ProviderFirstName,
                                                                                ProviderLastName = pl.ProviderLastName,
                                                                                ProviderFullName = pl.ProviderFullName,
                                                                                LocationPracticeAddress = pl.ServiceAddress1 + " " + (string.IsNullOrEmpty(pl.ServiceAddress2) ? " " : pl.ServiceAddress2) +
                                                                                                        " " + pl.ServiceCity + " " + pl.ServiceState + " " + pl.ServiceZip.Substring(0, 5),
                                                                                ServiceAddress1 = pl.ServiceAddress1,
                                                                                ServiceAddress2 = pl.ServiceAddress2,
                                                                                ServiceCity = pl.ServiceCity,
                                                                                ServiceState = pl.ServiceState,
                                                                                ServiceZip = pl.ServiceZip,
                                                                                Specialization = _providerRepository.GetSpecialityById(pl.ProviderSpecialityCode).Speciality,
                                                                                Facility = string.IsNullOrEmpty(pl.ProviderCategory) ? string.Empty : _providerRepository.GetFacilityById(pl.ProviderCategory).ProviderCategory,
                                                                                PhoneAreaCode = string.IsNullOrEmpty(pl.PhoneAreaCode) ? string.Empty : pl.PhoneAreaCode,
                                                                                PhoneNumber = string.IsNullOrEmpty(pl.PhoneNumber) ? string.Empty : pl.PhoneNumber,
                                                                                PhoneExtension = string.IsNullOrEmpty(pl.PhoneExtension) ? string.Empty : "(" + pl.PhoneExtension + ")",
                                                                                Latitude = string.IsNullOrEmpty(pl.Latitude) ? 0 : Convert.ToDouble(pl.Latitude),
                                                                                Longitude = string.IsNullOrEmpty(pl.Longitude) ? 0 : Convert.ToDouble(pl.Longitude),
                                                                                ProviderType = string.IsNullOrEmpty(pl.ProviderCategory) ? Constants.Ind : _providerRepository.GetFacilityById(pl.ProviderCategory).Type,
                                                                                ProviderCategory = pl.ProviderCategory
                                                                            })).Skip((pageNumber - 1) * _configurationKeys.Value.PaginationLimit).Take(_configurationKeys.Value.PaginationLimit).ToList();
                        return ProviderTopSliceList.ToList();
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                loggers.Error($"{Constants.PWeb} failed to get all organization details.");
                return null;
            }
        }
        public async Task<List<ProviderResultDetails>> GetFilteredOrganizationDetails(string networkCode, string zipCode, int miles, int pageNumber, string specialization, string facility, string providerOrFacilityName, int providerId)
        {
            try
            {
                string collectionName = await _organizationRepository.GetCollectionName(networkCode);
                if (collectionName != null)
                {
                    List<PEntity> providerList = await _providerRepository.GetAllOrganizationDetails(false, collectionName);

                    if (!string.IsNullOrEmpty(facility))
                        providerList = providerList.Where(provider => provider.ProviderCategory != null && provider.ProviderCategory.ToLower().Equals(facility.ToLower())).ToList();

                    if (!string.IsNullOrEmpty(specialization))
                        providerList = providerList.Where(provider => provider.ProviderSpecialityCode != null && provider.ProviderSpecialityCode.ToLower().Equals(specialization.ToLower())).ToList();

                    if (!string.IsNullOrEmpty(providerOrFacilityName))
                        providerList = providerList.Where(provider => (provider.ProviderFirstName.ToLower().Contains(providerOrFacilityName.ToLower()) || provider.ProviderLastName.ToLower().Contains(providerOrFacilityName.ToLower()) || provider.ProviderFullName.ToLower().Contains(providerOrFacilityName.ToLower()))).ToList();

                    List<ProviderResultDetails> ProviderTopSliceList = (from pl in providerList
                                                                        select (new ProviderResultDetails
                                                                        {
                                                                            Id = pl.IdGeneratedNumber,
                                                                            ProviderFirstName = pl.ProviderFirstName,
                                                                            ProviderLastName = pl.ProviderLastName,
                                                                            ProviderFullName = pl.ProviderFullName,
                                                                            LocationPracticeAddress = pl.ServiceAddress1 + " " + (string.IsNullOrEmpty(pl.ServiceAddress2) ? " " : pl.ServiceAddress2) +
                                                                                                        " " + pl.ServiceCity + " " + pl.ServiceState + " " + pl.ServiceZip.Substring(0, 5),
                                                                            ServiceAddress1 = pl.ServiceAddress1,
                                                                            ServiceAddress2 = pl.ServiceAddress2,
                                                                            ServiceCity = pl.ServiceCity,
                                                                            ServiceState = pl.ServiceState,
                                                                            ServiceZip = pl.ServiceZip,
                                                                            Specialization = _providerRepository.GetSpecialityById(pl.ProviderSpecialityCode).Speciality,
                                                                            Facility = string.IsNullOrEmpty(pl.ProviderCategory) ? string.Empty : _providerRepository.GetFacilityById(pl.ProviderCategory).ProviderCategory,
                                                                            PhoneAreaCode = string.IsNullOrEmpty(pl.PhoneAreaCode) ? string.Empty : pl.PhoneAreaCode,
                                                                            PhoneNumber = string.IsNullOrEmpty(pl.PhoneNumber) ? string.Empty : pl.PhoneNumber,
                                                                            PhoneExtension = string.IsNullOrEmpty(pl.PhoneExtension) ? string.Empty : "(" + pl.PhoneExtension + ")",
                                                                            Latitude = string.IsNullOrEmpty(pl.Latitude) ? 0 : Convert.ToDouble(pl.Latitude),
                                                                            Longitude = string.IsNullOrEmpty(pl.Longitude) ? 0 : Convert.ToDouble(pl.Longitude),
                                                                            ProviderType = string.IsNullOrEmpty(pl.ProviderCategory) ? Constants.Ind : _providerRepository.GetFacilityById(pl.ProviderCategory).Type,
                                                                            ProviderCategory = pl.ProviderCategory
                                                                        })).ToList().Where(item => item.Id > providerId).OrderBy(item => item.Id).ToList();

                    var sourceLocation = (dynamic)null;
                    if (!string.IsNullOrEmpty(zipCode)) 
                    {
                        //Request the Google API when the zipcode has value
                        sourceLocation = await GeoCoordinateTool.GetLocation(zipCode, _configurationKeys.Value.Google_Map_Api_Url, _configurationKeys.Value.Google_Map_Api_Key);
                    }
                    List<ProviderResultDetails> providerResults = new List<ProviderResultDetails>();

                    int counter = 0;
                    foreach (var location in ProviderTopSliceList)
                    {
                        if (counter == _configurationKeys.Value.PaginationLimit)
                            break;

                        if (!string.IsNullOrEmpty(zipCode))
                        {
                            Tuple<bool, string> getLocationValues = IsLocationWithInRange(miles, sourceLocation, new LocationEntity() { Latitude = Convert.ToDouble(location.Latitude), Longitude = Convert.ToDouble(location.Longitude) });
                            if (getLocationValues.Item1 == true && getLocationValues != null)
                            {
                                location.RadiusDistance = getLocationValues.Item2;
                                providerResults.Add(location);
                                counter++;
                            }
                        }
                        else
                        {
                            providerResults.Add(location);
                            counter++;
                        }
                    }

                    return providerResults;
                }
                return null;
            }
            catch (Exception ex)
            {
                loggers.Error($"{Constants.PWeb} failed to get all filtered organization details.");
                return null;
            }
        }
        public List<ConfirmationEntity> PrepareGroupEntity(List<GroupEntity> groupEntityList)
        {
            List<ConfirmationEntity> lst = new List<ConfirmationEntity>();
            ConfirmationEntity ce = new ConfirmationEntity();
            var GroupList = from gel in groupEntityList
                            from pl in gel.Plans
                            from net in pl.Networks
                            select (new ConfirmationEntity
                            {
                                PlanName = pl.Name,
                                NetworkCode = net.Code,
                                GroupId = gel.GroupId,
                                GroupName = gel.Name,
                                GroupNumber = gel.Number,
                                OrganizationId = gel.Organization.OrganizationId
                            });
            int count = 1;
            return GroupList
           .Select(pl => new ConfirmationEntity()
           {
               PlanName = pl.PlanName,
               NetworkCode = pl.NetworkCode,
               GroupId = pl.GroupId,
               GroupName = pl.GroupName,
               GroupNumber = pl.GroupNumber,
               OrganizationId = pl.OrganizationId,
               Option = count++
           })
           .ToList();
        }
        private Tuple<bool, string> IsLocationWithInRange(int rangeInMiles, LocationEntity source, LocationEntity target)
        {
            if (source == null || target == null)
                return null;

            var sourceCoordinates = new GeoCoordinate(source.Latitude, source.Longitude);
            var targetCoordinates = new GeoCoordinate(target.Latitude, target.Longitude);
            var distance = (GeoCoordinateTool.Distance(sourceCoordinates, targetCoordinates, 1)); //1 - For MILES, 2 - For KM
            string distanceInSubstring = distance.ToString().Substring(0, 4);
            return Tuple.Create(distance <= (rangeInMiles) ? true : false, distanceInSubstring);
        }
        public async Task<List<SpecialityListEntity>> GetAllSpecialityList(int pageNumber, string collectionName)
        {
            try
            {
                var refreshData = pageNumber == 1 ? true : false;
                string partnerName = await _organizationRepository.GetCollectionName(collectionName);
                if (partnerName != null)
                {
                    return await _providerRepository.GetAllSpecialityList(partnerName, refreshData);
                }
                return null;
            }
            catch (Exception ex)
            {
                loggers.Error($"{Constants.PWeb} failed to get all specialities.");
                return null;
            }
        }
        public async Task<List<ProviderCategoryListEntity>> GetAllFacilityList(int pageNumber, string collectionName)
        {
            try
            {
                var refreshData = pageNumber == 1 ? true : false;
                string partnerName = await _organizationRepository.GetCollectionName(collectionName);
                if (!String.IsNullOrEmpty(partnerName))
                {
                    return await _providerRepository.GetAllFacilityList(partnerName, refreshData);
                }
                return null;
            }
            catch (Exception ex)
            {
                loggers.Error($"{Constants.PWeb} failed to get all facilities.");
                return null;
            }
        }
    }
}
