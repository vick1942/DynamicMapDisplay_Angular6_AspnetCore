using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IRepository
{
    public interface IOrganizationRepository
    {
        Task<string> GetCollectionName(string networkCode);
    }
}
