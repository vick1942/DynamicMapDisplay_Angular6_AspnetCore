using Business.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IRepository
{
    public interface IGroupRepository
    {
        Task<List<GroupEntity>> GetPlanDetailsByName(string name);
        Task<List<GroupEntity>> GetGroupDetailsByNameOrNumber(string nameOrNumber);
        Task<List<PlanEntity>> GetAllPlanDetails();
        Task<List<GroupEntity>> GetAllGroupDetails();
    }
}
