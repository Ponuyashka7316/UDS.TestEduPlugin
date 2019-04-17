using System;
using Microsoft.Xrm.Sdk;
using UDS.GurinPlugin.Repository;

namespace UDS.GurinPlugin
{
    public class TestEduPlugin : Plugin
    {
        public TestEduPlugin()
            : base(typeof(TestEduPlugin))
        {
            base.RegisteredEvents
                .Add(new Tuple<int, string, string, Action<LocalPluginContext>>((int)PluginStage.PostOperation, "Create",
                    "new_mainentity",
                    PullThePlugin));
        }

        protected void PullThePlugin(LocalPluginContext localContext)
        {
            if (!localContext.PluginExecutionContext.InputParameters.Contains("Target"))
            {
                return;
            }
            Entity target = (Entity)localContext.PluginExecutionContext.InputParameters["Target"];
            localContext.Trace("");
            //Entity postEntity = localContext.PluginExecutionContext.PostEntityImages["PostImage"];

            decimal dec = target.GetAttributeValue<decimal>("new_associatedcontacts");
            //Money cur = target.GetAttributeValue<Money>("new_decimal");
            //EntityReference lookupRef = target.GetAttributeValue<EntityReference>("new_decimal");
            IOrganizationService service = localContext.OrganizationService;
            target["new_decimal"] = 112;
            Entity someNewEnt = new Entity(target.LogicalName, target.Id);
            someNewEnt["new_decimal"] = dec + 10;
            service.Update(someNewEnt);

            /*
            if (!localContext.PluginExecutionContext.PostEntityImages.Contains(postImageName))
            {
                throw new InvalidPluginExecutionException("There is no PostImage. Need to register step correctly");
            }
            */
        }
    }
}
