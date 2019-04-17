using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDS.GurinPlugin
{
    public class Gu_1_1 : Plugin
    {
        public Gu_1_1()
            : base(typeof(Gu_1_1))
        {
            base.RegisteredEvents
                .Add(new Tuple<int, string, string, Action<LocalPluginContext>>((int)PluginStage.PreOperation, "Create",
                    "myprefix_gu_main",
                    AddTextPlugin));
        }

        private void AddTextPlugin(LocalPluginContext localContext)
        {
            if (!localContext.PluginExecutionContext.InputParameters.Contains("Target"))
            {
                return;
            }
            Entity target = (Entity)localContext.PluginExecutionContext.InputParameters["Target"];
            //throw new InvalidPluginExecutionException("тест");
            target["new_associatedcontacts"] = new Random().Next().ToString();

        }
    }
}
