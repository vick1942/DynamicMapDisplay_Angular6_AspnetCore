using Business.Entities.Entities;
using Common.Constants;
using IRepository;
using Repository.Entities.Entities;
using Repository.Utilities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class OrganizationRepository : IOrganizationRepository
    {
        Logger loggers = LogManager.GetLogger(Constants.PWeb);
        private readonly DataAccess _context = null;
        public static List<ProviderEntity> ProviderTempDB = new List<ProviderEntity>();
        public static Cache cacheDetail = new Cache();

        public OrganizationRepository(IOptions<DbSettings> dbSettings)
        {
            _context = new DataAccess(dbSettings);
        }
        public async Task<string> GetCollectionName(string networkCode)
        {
            try
            {
                if (cacheDetail.NetworkCode != networkCode)
                {
                    var OrganizationbQuery = "{'Networks':{$elemMatch:{Code:'" + networkCode + "'}}}";
                    OrganizationCollection organizationDetails = await _context.OrganizationCollection
                    .Find(OrganizationbQuery).FirstOrDefaultAsync();

                    if (organizationDetails != null)
                    {
                        cacheDetail.NetworkCode = networkCode;
                        string collectionName = organizationDetails.Networks.Where(x => x.Code == networkCode).FirstOrDefault().Name;
                        cacheDetail.CollectionName = collectionName;
                        return collectionName;
                    }
                    return null;
                }
                return cacheDetail.CollectionName;
            }
            catch (Exception ex)
            {
                loggers.Error(ex, $"An exception occurred while trying to get the collection name for the network: {networkCode} - {ex.Message}");
                return null;
            }
        }
    }
}
