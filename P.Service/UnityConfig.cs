using Business;
using IBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace P.Service
{
    public static class UnityConfig
    {
        public static void RegisterComponents(UnityContainer container)
        {
            //UnityContainer container = new UnityContainer();
            //container.RegisterType<IImportService, ImportService>();
        }
    }
}
