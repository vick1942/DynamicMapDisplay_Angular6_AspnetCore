using Business.Entities;
using Business.Entities.Entities;
using Common.Constants;
using Common.Mappers;
using IRepository;
using Repository.Entities;
using Repository.Entities.Entities;
using Repository.Utilities;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class GroupRepository : IGroupRepository
    {
        private readonly DataAccess _context = null;
        public static List<ProviderEntity> ProviderTempDB = new List<ProviderEntity>();
        public static Cache cacheDetail = new Cache();
        Logger loggers = LogManager.GetLogger(Constants.PWeb);
        public GroupRepository(IOptions<DbSettings> dbSettings)
        {
            _context = new DataAccess(dbSettings);
        }
        public async Task<List<GroupEntity>> GetPlanDetailsByName(string name)
        {
            try
            {
                var bQuery = "{'Plans':{$elemMatch:{Name: /.*" + name + ".*/i}}}";
                var filter = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(bQuery);
                List<Group> planList = await _context.GroupCollection.Find
                               (filter).ToListAsync();
                var planDetailList = ProviderEntityMapper<Group, GroupEntity>.MapEntityCollection(planList);
                return planDetailList;
            }
            catch (Exception ex)
            {
                loggers.Error(ex, $"An exception occurred while trying to get plan details by name for {name} - {ex.Message}");
                return null;
            }
        }
        public async Task<List<GroupEntity>> GetGroupDetailsByNameOrNumber(string nameOrNumber)
        {
            try
            {
                var filter = Builders<Group>.Filter.Regex(s => s.Number, new BsonRegularExpression(nameOrNumber, "i"))
                    | Builders<Group>.Filter.Regex(s => s.Name, new BsonRegularExpression(nameOrNumber, "i"));
                List<Group> groupByName = await _context.GroupCollection.Find(filter).ToListAsync();
                var groupDBList = ProviderEntityMapper<Group, GroupEntity>.MapEntityCollection(groupByName);
                return groupDBList;
            }
            catch (Exception ex)
            {
                loggers.Error(ex, $"An exception occurred while trying to get group details by name  or number for {nameOrNumber} - {ex.Message}");
                return null;
            }
        }
        public async Task<List<GroupEntity>> GetAllGroupDetails()
        {
            try
            {
                List<Group> groupList = await _context.GroupCollection
                               .Find(m => true).ToListAsync();
                var groupDBList = ProviderEntityMapper<Group, GroupEntity>.MapEntityCollection(groupList);
                return groupDBList;
            }
            catch (Exception ex)
            {
                loggers.Error(ex, $"Failed to get all the group details - {ex.Message}");
                return null;
            }
        }
        public async Task<List<PlanEntity>> GetAllPlanDetails()
        {
            try
            {
                List<Group> planList = await _context.GroupCollection
                               .Find(m => true).ToListAsync();
                var planDBList = ProviderEntityMapper<Group, PlanEntity>.MapEntityCollection(planList);
                return planDBList;
            }
            catch (Exception ex)
            {
                loggers.Error(ex, $"Failed to get all the plan details - {ex.Message}");
                return null;
            }
        }
    }
}
