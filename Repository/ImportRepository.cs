using Business.Entities;
using Business.Entities.Entities;
using Common.Constants;
using IRepository;
using Repository.Entities.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ImportRepository : IImportRepository
    {
        Logger loggers = LogManager.GetLogger(Constants.PServiceLogger);
        private readonly IMongoDatabase _database = null;
        public ImportRepository(string connectionString, string database)
        {
            var client = new MongoClient(MongoUrl.Create(connectionString));
            if (client != null)
                _database = client.GetDatabase(database);
        }
        public async Task SaveProviderCollection(List<PEntity> providers, string networkName, string fileName, int bulkInsertRecordsCount)
        {
            try
            {
                var counter = bulkInsertRecordsCount;
                _database.DropCollection(networkName);
                loggers.Debug($"{networkName} collection has been droped.");
                _database.CreateCollection(networkName);
                loggers.Debug($"{networkName} collection has been created.");

                var newCollection = _database.GetCollection<BsonDocument>(networkName);
                List<BsonDocument> doc = new List<BsonDocument>();
                var chunkList = Common.ChunkTool.splitList(providers, bulkInsertRecordsCount);
                loggers.Debug($"Total number of records received for the file {fileName} are {providers.Count()}. Total chunk count: {chunkList.Count()}.");

                foreach (var list in chunkList)
                {
                    foreach (var provider in list)
                    {
                        var documnt = new BsonDocument
                        {
                             { "ProviderTIN" , provider.ProviderTIN},
                              { "ProviderFullName" , provider.ProviderFullName},
                              { "BillingAddress1" , provider.BillingAddress1},
                              { "BillingAddress2" , provider.BillingAddress2},
                              { "BillingCity" , provider.BillingCity},
                              { "BillingState" , provider.BillingState},
                              { "BillingZip" , provider.BillingZip},
                              { "PhoneAreaCode" , provider.PhoneAreaCode},
                              { "PhoneNumber" , provider.PhoneNumber},
                              { "PhoneExtension" , provider.PhoneExtension},
                              { "ProviderSpecialityCode" , provider.ProviderSpecialityCode},
                              { "ServiceAddress1" , provider.ServiceAddress1},
                              { "ServiceAddress2" , provider.ServiceAddress2},
                              { "ServiceCity" , provider.ServiceCity},
                              { "ServiceState" , provider.ServiceState},
                              { "ServiceZip" , provider.ServiceZip},
                              { "ProviderCategory" , provider.ProviderCategory},
                              { "ProviderFirstName" , provider.ProviderFirstName},
                              { "ProviderLastName" , provider.ProviderLastName},
                              { "ProviderNPI" , provider.ProviderNPI},
                              { "ProviderFeeScheduleIdentifier" , provider.ProviderFeeScheduleIdentifier},
                              { "ProviderGroupAssignment" , provider.ProviderGroupAssignment},
                              { "EffectiveDate" , provider.EffectiveDate},
                              { "TerminationDate" , provider.TerminationDate},
                              { "DiscountPercentage" , provider.DiscountPercentage},
                              { "Endorsedornonendorsed" , provider.Endorsedornonendorsed},
                              { "SecondProviderSpecialtyifapplicable" , provider.SecondProviderSpecialtyifapplicable},
                              { "ContractName" , provider.ContractName},
                              { "EPOidentifier" , provider.EPOidentifier},
                              { "MiddleInitialIfApplicable" , provider.MiddleInitialIfApplicable},
                              { "Latitude" , provider.Latitude},
                              { "Longitude" , provider.Longitude }
                        };
                        doc.Add(documnt);
                    }
                    counter += bulkInsertRecordsCount;
                    await newCollection.InsertManyAsync(doc);
                    doc = new List<BsonDocument>();
                    loggers.Debug($"{counter} records have been inserted into DB for the file {fileName}");
                }

                loggers.Debug($"Records have been saved successfully for the file {fileName}.");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> IsOrganizationCodeExists(string organizationCode)
        {
            try
            {
                IMongoCollection<BsonDocument> collection = _database.GetCollection<BsonDocument>("Organization");
                BsonDocument filter = new BsonDocument();
                filter.Add("Code", organizationCode);

                BsonDocument projection = new BsonDocument();
                projection.Add("Code", 1);

                var options = new FindOptions<BsonDocument>()
                {
                    Projection = projection
                };
                using (var cursor = await collection.FindAsync(filter, options).ConfigureAwait(false))
                {
                    while (await cursor.MoveNextAsync().ConfigureAwait(false))
                    {
                        var batch = cursor.Current;
                        foreach (BsonDocument document in batch)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public async Task<string> GetNetworkNameByCode(string networkCode, string organizationCode)
        {
            try
            {
                IMongoCollection<BsonDocument> collection = _database.GetCollection<BsonDocument>("Organization");
                BsonDocument filter = new BsonDocument();
                filter.Add("Code", organizationCode);
                filter.Add("Networks.Code", networkCode);

                BsonDocument projection = new BsonDocument();
                projection.Add("Networks.Name", 1);
                projection.Add("Networks.Code", 1);

                string networkName = string.Empty;

                var options = new FindOptions<BsonDocument>()
                {
                    Projection = projection
                };
                using (var cursor = await collection.FindAsync(filter, options).ConfigureAwait(false))
                {
                    while (await cursor.MoveNextAsync().ConfigureAwait(false))
                    {
                        var batch = cursor.Current;
                        foreach (BsonDocument document in batch)
                        {
                            foreach (BsonDocument network in document["Networks"].AsBsonArray)
                            {
                                if (network["Code"].ToString().ToLower().Equals(networkCode.ToLower()))
                                {
                                    networkName = network["Name"].ToString();
                                    return networkName;
                                }
                            }
                        }
                    }
                }

                return networkName;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public async Task<List<ProviderCategoryEntity>> GetAllProviderCategories()
        {
            try
            {
                var collection = _database.GetCollection<ProviderCategoryEntity>("ProviderCategory");
                return collection.AsQueryable().ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<SpecialityEntity>> GetAllSpecialities()
        {
            try
            {
                var collection = _database.GetCollection<SpecialityEntity>("Speciality");
                return collection.AsQueryable().ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task UpdateSystemStatus()
        {
            SystemStatus st = new SystemStatus();
            st.LastPingDate = DateTime.UtcNow;
            st.SystemName = Constants.PServiceLogger;
            await _database.GetCollection<SystemStatus>("SystemStatus").ReplaceOneAsync(s => s.SystemName == st.SystemName, st);
        }
    }
}
