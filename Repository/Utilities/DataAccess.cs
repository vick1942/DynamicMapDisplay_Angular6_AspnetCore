using Repository.Entities;
using Repository.Entities.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Repository.Utilities
{
    public class DataAccess
    {
        private readonly IMongoDatabase _database = null;
        private readonly IOptions<DbSettings> _dbSettings = null;
        public DataAccess(IOptions<DbSettings> dbSettings)
        {
            _dbSettings = dbSettings;
            var client = new MongoClient(_dbSettings.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(_dbSettings.Value.Database);
        }
        public IMongoCollection<Group> GroupCollection
        {
            get
            {
                return _database.GetCollection<Group>(_dbSettings.Value.GroupCollection);
            }
        }
        public IMongoCollection<OrganizationCollection> OrganizationCollection => _database.GetCollection<OrganizationCollection>(_dbSettings.Value.OrganizationCollection);
        public IMongoCollection<P> EliteCollection(string collectionName) => _database.GetCollection<P>(collectionName);
        public IMongoCollection<SpecialityList> SpecialityCollection(string collectionName) => _database.GetCollection<SpecialityList>(_dbSettings.Value.SpecialityCollection);
        public IMongoCollection<ProviderCategoryList> ProviderCategoryCollection(string collectionName) => _database.GetCollection<ProviderCategoryList>(_dbSettings.Value.ProviderCategoryCollection);
    }
}
