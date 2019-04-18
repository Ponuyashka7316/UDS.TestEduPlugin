using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;

namespace UDS.GurinPlugin
{
    public class CustomStep : CodeActivity
    {
        #region Workflow parameters
        [RequiredArgument]
        [Output("GuOutParam")]
        public OutArgument<string> OutParam { get; set; }

        [RequiredArgument]
        [Input("Account")]
        [ReferenceTarget("account")]
        public InArgument<EntityReference> Account { get; set; }
        #endregion

        protected override void Execute(CodeActivityContext executionContext)
        {
            var context = executionContext.GetExtension<IWorkflowContext>();
            var serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            var service = serviceFactory.CreateOrganizationService(context.UserId);
            String accountEmail = null;

            Entity updatedEntity = new Entity(context.PrimaryEntityName);
            updatedEntity.Id = context.PrimaryEntityId;
            updatedEntity["new_wholenumberfield"] = 123;
            EntityReference accountReference = Account.Get<EntityReference>(executionContext);
            if (accountReference != null)
            {
                string[] srt = { "emailaddress1" };
                Entity account = service.Retrieve(accountReference.LogicalName, accountReference.Id, new ColumnSet(srt));
                if (account.GetAttributeValue<string>("emailaddress1") != null)
                {
                    accountEmail = account.GetAttributeValue<string>("emailaddress1");
                }

                OutParam.Set(executionContext, "Електронна пошта:  " + accountEmail);
            }
            service.Update(updatedEntity);
        }

    }
}
