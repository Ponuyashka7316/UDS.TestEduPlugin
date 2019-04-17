using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDS.GurinPlugin
{
   public class GuTask_3_1 : Plugin
    {

        public GuTask_3_1()
          : base(typeof(GuTask_3_1))
        {
            base.RegisteredEvents
                .Add(new Tuple<int, string, string, Action<LocalPluginContext>>((int)PluginStage.PreOperation, "Update",
                    "myprefix_gu_main",
                    PullThePlugin));
        }
        /*1)	На форме сущности А должны используем 2 поля: Клиент и Контакт. Когда изменяем клиент,
         * в поле Контакт подставляется основной контакт этого клиента. Когда изменяется контакт, в поле Клиент
         * подставляется связанная организация по этому контакту.*/
        private void PullThePlugin(LocalPluginContext localContext)
        {
            Entity target = (Entity)localContext.PluginExecutionContext.InputParameters["Target"];
            EntityReference contactReference = target.GetAttributeValue<EntityReference>("new_contact");
            EntityReference companyReference = target.GetAttributeValue<EntityReference>("new_lookupfield");
            IOrganizationService service = localContext.OrganizationService;
            if (contactReference != null)
            {
                string[] srt = { "parentcustomerid" };
                Entity contactCompanyName = service.Retrieve(contactReference.LogicalName, contactReference.Id, new ColumnSet(srt));
                target["new_lookupfield"] = contactCompanyName.GetAttributeValue<EntityReference>("parentcustomerid");
            }
            else if (companyReference != null)
            {
                string[] srt = { "primarycontactid" };
                Entity companyPrimaryContact = service.Retrieve(companyReference.LogicalName, companyReference.Id, new ColumnSet(srt));
                target["new_contact"] = companyPrimaryContact.GetAttributeValue<EntityReference>("primarycontactid");
            }

        }
    }
}
