using IBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace P.Service
{
    public class EventChecker : System.ServiceProcess.ServiceBase
    {
        private static IUnityContainer _container;

        public EventChecker(IUnityContainer container)
        {
            _container = container;
        }

        public void SomeOperationThatGetsTriggeredByATimer()
        {
            //using (_container.StartSomeScope())
            //{
            //    var service = _container.Resolve<IImportService>();

            //    service.Process();
            //}
        }
    }
}
