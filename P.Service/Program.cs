using Business;
using IBusiness;
using IRepository;
using Repository;
using Repository.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Unity.Injection;

namespace P.Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            var container = new UnityContainer();
            container.RegisterType<IImportService, ImportService>();
            container.RegisterType<IImportRepository, ImportRepository>();
            container.RegisterType<ImportRepository>(new InjectionConstructor(new object[] { System.Configuration.ConfigurationManager.ConnectionStrings["P"].ConnectionString, System.Configuration.ConfigurationManager.AppSettings["MongoDbName"].ToString() }));
            UnityConfig.RegisterComponents(container);

            System.IO.Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
#if SERVICE
           
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                container.Resolve<AutomateImportService>().TestStartupAndStopAsync(args).Wait()
                //new AutomateImportService()
            };
            ServiceBase.Run(ServicesToRun);
#else
            //container.Resolve<AutomateImportService>().TestStartupAndStopAsync(args).Wait();
            AutomateImportService service = container.Resolve<AutomateImportService>();
            service.TestStartupAndStop(args);
#endif

        }
    }
}
