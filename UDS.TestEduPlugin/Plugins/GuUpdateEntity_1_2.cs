using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDS.GurinPlugin
{
    public class GuUpdateEntity_1_2 : Plugin
    {
        public GuUpdateEntity_1_2()
            : base(typeof(GuUpdateEntity_1_2))
        {
            base.RegisteredEvents
                .Add(new Tuple<int, string, string, Action<LocalPluginContext>>((int)PluginStage.PostOperation, "Update",
                    "myprefix_gu_main",
                    UpdateField));
        }

        /*2)	Написать плагин, кот. срабатывает на обновление  у сущности А поля Клиент и выбрасывает исключение,
         * если у заданного клиента не заполнен электронный адрес. Если е-мейл есть, то заполняется информационное поле с 
         * текстом «Клиент соответствует требованиям».
            Примечание: получить поле нужной записи, зная её Id, можно с помощью метода Retrieve у OrganizatonService
        */
        private void UpdateField(LocalPluginContext localContext)
        {
            Entity target = (Entity)localContext.PluginExecutionContext.InputParameters["Target"];
            if (!localContext.PluginExecutionContext.InputParameters.Contains("Target"))
            {
                return;
            }
            EntityReference contactReference = target.GetAttributeValue<EntityReference>("new_contact");
            IOrganizationService service = localContext.OrganizationService;

            if (contactReference != null)
            {
                string[] srt = { "emailaddress1" };
                Entity linkedEntity = service.Retrieve(contactReference.LogicalName, contactReference.Id, new ColumnSet(srt));
                Entity newTempEntity = new Entity(target.LogicalName, target.Id);

                if (linkedEntity.GetAttributeValue<string>("emailaddress1") != null)
                {
                    newTempEntity["new_associatedcontacts"] = "Customer meets the requirements";
                    service.Update(newTempEntity);
                }
                else
                {
                    newTempEntity["new_associatedcontacts"] = "Customer does not meet the requirements";
                    localContext.Trace("test trace");
                    service.Update(newTempEntity);
                    //throw new InvalidPluginExecutionException("тест");
                }
            }
        }
    }
}
