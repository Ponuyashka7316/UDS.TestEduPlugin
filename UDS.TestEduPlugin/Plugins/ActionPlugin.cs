using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UDS.GurinPlugin.Repository;

namespace UDS.GurinPlugin
{
    public class ActionPlugin : Plugin
    {
        public ActionPlugin()
            : base(typeof(ActionPlugin))
        {
            base.RegisteredEvents
                .Add(new Tuple<int, string, string, Action<LocalPluginContext>>((int)PluginStage.PostOperation, "new_Gu_Action",
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
            Guid currentEntityId = Guid.Parse(localContext.PluginExecutionContext.InputParameters["guentity"].ToString());
            DeactivateRelatedEntitiesRepository deact = new DeactivateRelatedEntitiesRepository(service);
            deact.DeactivateRecords(currentEntityId);
            //var actionResponse = (OrganizationResponse)service.Execute(actionrequest);
        }
    }
}
