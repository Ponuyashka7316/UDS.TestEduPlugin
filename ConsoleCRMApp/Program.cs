using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KostenVoranSchlagConsoleParser.Helpers;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using UDS.GurinPlugin;

namespace ConsoleCRMApp
{
    class Program
    {
        static void Main(string[] args)
        {
            OrganizationServiceProxy serviceProxy = ConnectHelper.CrmService;
            var service = (IOrganizationService)serviceProxy;
            Entity someEnt = service.Retrieve("myprefix_gu_main", new Guid("EEACCB31-AD40-E911-8115-00155D05FA01"), new ColumnSet(true));
            ServiceClass serviceClass = new ServiceClass(service);
            serviceClass.MainMethod();
            string var = null;
        }
    }
}
