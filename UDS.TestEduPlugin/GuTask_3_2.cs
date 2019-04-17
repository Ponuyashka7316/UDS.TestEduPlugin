using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDS.GurinPlugin
{
    public class GuTask_3_2 : Plugin
    {
        public GuTask_3_2()
        : base(typeof(GuTask_3_2))
        {
            base.RegisteredEvents
                .Add(new Tuple<int, string, string, Action<LocalPluginContext>>((int)PluginStage.PostOperation, "Update",
                    "contact",
                    PullThePlugin));
        }


        /*2)	Написать плагин на деактивацию контакта. Если к этому контакту была привязана тестовая сущность А, 
         * то после деактивации контакта выводить в информационное поле А: в системе был деактивирован контакт с 
         * e-mail: [e-mail удаленного контакта]. Записи в информационном поле записи не затирать, а добавлять по
         * методу стека: новые вверху.*/
        private void PullThePlugin(LocalPluginContext localContext)
        {
            Entity target = (Entity)localContext.PluginExecutionContext.InputParameters["Target"];
            if (!localContext.PluginExecutionContext.PostEntityImages.Contains("GuImage"))
            {
                throw new InvalidPluginExecutionException("image does not exist");
            }
            Entity image = localContext.PluginExecutionContext.PostEntityImages["GuImage"];

            EntityReference guMainReference = image.GetAttributeValue<EntityReference>("new_guentityid");
            IOrganizationService service = localContext.OrganizationService;
            Entity newTempEntity = new Entity(target.LogicalName, target.Id);

            if (guMainReference != null)
            {
                newTempEntity["statecode"] = new OptionSetValue(1);
                newTempEntity["statuscode"] = new OptionSetValue(2);    //should use SetStateRequest
                Entity recordToUpdate = service.Retrieve(guMainReference.LogicalName, guMainReference.Id, new ColumnSet(true));
                var temp = image.GetAttributeValue<string>("emailaddress1");
                recordToUpdate["new_associatedcontacts"] = temp + "\t\n" + recordToUpdate.GetAttributeValue<string>("new_associatedcontacts");
                service.Update(recordToUpdate);
                service.Update(newTempEntity);

            }

        }
    }
}
