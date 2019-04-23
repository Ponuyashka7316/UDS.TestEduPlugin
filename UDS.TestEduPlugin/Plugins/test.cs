using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDS.GurinPlugin.Plugins
{
    
        public class test : Plugin
        {
            public test()
                : base(typeof(test))
            {
                base.RegisteredEvents
                    .Add(new Tuple<int, string, string, Action<LocalPluginContext>>((int)PluginStage.PostOperation, "new_test",
                        null,
                        PullThePlugin));
            }

            protected void PullThePlugin(LocalPluginContext localContext)
            {
                if (!localContext.PluginExecutionContext.InputParameters.Contains("guentity"))
                {
                    return;
                }
                IOrganizationService service = localContext.OrganizationService;
            }
       }
}
