using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDS.GurinPlugin
{
    public class GuTask_3_3:Plugin
    {
        public GuTask_3_3()
       : base(typeof(GuTask_3_3))
        {
            base.RegisteredEvents
                .Add(new Tuple<int, string, string, Action<LocalPluginContext>>((int)PluginStage.PreOperation, "Delete",
                    "contact",
                    PullThePlugin));
        }
        /*
         3)	Написать плагин на удаление контакта. Если к этому контакту была привязана тестовая сущность А,
         то после удаления контакта выводить в информационное поле А: из системы был удален контакт с e-mail:
         [e-mail удаленного контакта].
             */
        private void PullThePlugin(LocalPluginContext localContext)
        {
            Entity target = (Entity)localContext.PluginExecutionContext.InputParameters["Target"];
            if (!localContext.PluginExecutionContext.PreEntityImages.Contains("GuImage"))
            {
                throw new InvalidPluginExecutionException("image does not exist");
            }
            Entity image = localContext.PluginExecutionContext.PreEntityImages["GuImage"];
            EntityReference guMainReference = image.GetAttributeValue<EntityReference>("new_guentityid");
            IOrganizationService service = localContext.OrganizationService;
            Entity someNewEnt = new Entity(target.LogicalName, target.Id);
            if (guMainReference != null)
            {
                Entity recordToUpdate = service.Retrieve(guMainReference.LogicalName, guMainReference.Id, new ColumnSet(true));
                var emailAddress = image.GetAttributeValue<string>("emailaddress1");
                recordToUpdate["new_associatedcontacts"] =  "\t\n из системы был удален контакт с e-mail " + emailAddress;
                service.Update(recordToUpdate);

            }
        }
    }
}
