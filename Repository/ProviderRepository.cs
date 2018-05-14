using Business.Entities.Entities;
using Common.Constants;
using Common.Mappers;
using IRepository;
using Repository.Entities.Entities;
using Repository.Utilities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Repository
{
    public class ProviderRepository : IProviderRepository
    {
        Logger loggers = LogManager.GetLogger(Constants.PServiceLogger);
        private readonly DataAccess _context = null;
        public static List<PEntity> ProviderTempDB = new List<PEntity>();
        public static List<ProviderCategoryListEntity> ProviderCategoryListTempDB = new List<ProviderCategoryListEntity>();
        public static List<SpecialityListEntity> SpecialityTempDB = new List<SpecialityListEntity>();
        public static Cache cacheDetail = new Cache();

        public ProviderRepository(IOptions<DbSettings> dbSettings)
        {
            _context = new DataAccess(dbSettings);
        }

        public async Task<List<PEntity>> GetAllOrganizationDetails(bool refresh, string collectionName)
        {
            try
            {
                if (ProviderTempDB.Count() == 0 || refresh)
                {
                    if (collectionName != null)
                    {
                        List<P> genericList = await _context.EliteCollection(collectionName)
                        .Find(m => true).ToListAsync();
                        if (genericList.Any())
                        {
                            int number = 0;
                            var eliteDBList = ProviderEntityMapper<P, PEntity>.MapEntityCollection(genericList);
                            List<PEntity> providerList = eliteDBList.Select(c => { c.IdGeneratedNumber = number++; return c; }).ToList();
                            ProviderTempDB = providerList;
                            return providerList;
                        }
                        return null;
                    }
                    return null;
                }
                else
                {
                    return ProviderTempDB.ToList();
                }
            }
            catch (Exception ex)
            {
                loggers.Error(ex, $"An exception occurred while trying to get all the Organization details for the collection: {collectionName} - {ex.Message}");
                return null;
            }
        }
        public async Task<List<ProviderCategoryListEntity>> GetAllFacilityList(string collectionName, bool refresh)
        {
            try
            {
                if (ProviderCategoryListTempDB.Count() == 0 || refresh)
                {
                    List<ProviderCategoryList> providerCategoryList = await _context.ProviderCategoryCollection(collectionName)
                           .Find(m => m.Code != string.Empty).ToListAsync();
                    if (providerCategoryList.Count > 0)
                    {
                        var groupDBList = ProviderEntityMapper<ProviderCategoryList, ProviderCategoryListEntity>.MapEntityCollection(providerCategoryList);
                        ProviderCategoryListTempDB = groupDBList;
                        return groupDBList;
                    }
                    return null;
                }
                else
                {
                    return ProviderCategoryListTempDB.ToList();
                }
            }
            catch (Exception ex)
            {
                loggers.Error(ex, $"Failed to fetch the dropdown details for the facility with the collectionName: {collectionName} - {ex.Message}");
                return null;
            }
        }
        public async Task<List<SpecialityListEntity>> GetAllSpecialityList(string collectionName, bool refresh)
        {
            try
            {
                if (SpecialityTempDB.Count() == 0 || refresh)
                {
                    List<SpecialityList> specialityList = await _context.SpecialityCollection(collectionName)
                           .Find(m => true).ToListAsync();
                    if (specialityList.Count > 0)
                    {
                        var groupDBList = ProviderEntityMapper<SpecialityList, SpecialityListEntity>.MapEntityCollection(specialityList);
                        SpecialityTempDB = groupDBList;
                        return groupDBList;
                    }
                    return null;
                }
                else
                {
                    return SpecialityTempDB.ToList();
                }
            }
            catch (Exception ex)
            {
                loggers.Error(ex, $"Failed to fetch the dropdown details for the speciality with the collectionName: {collectionName} - {ex.Message}");
                return null;
            }

        }
        public ProviderCategoryListEntity GetFacilityById(string code)
        {
            try
            {
                if (ProviderCategoryListTempDB.Count() != 0)
                {
                    return ProviderCategoryListTempDB.Where(x => x.Code.ToLower().Equals(code.ToLower())).FirstOrDefault();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                loggers.Error(ex, $"Failed to fetch the facility by ID for the Network code: {code} - {ex.Message}");
                return null;
            }
        }

        public SpecialityListEntity GetSpecialityById(string code)
        {
            try
            {
                if (SpecialityTempDB.Count() != 0)
                {
                    return SpecialityTempDB.Where(x => x.CodeNumber.ToLower().Equals(code.ToLower())).FirstOrDefault();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                loggers.Error(ex, $"Failed to fetch the speciality by ID for the Network code: {code} - {ex.Message}");
                return null;
            }
        }
    }
}