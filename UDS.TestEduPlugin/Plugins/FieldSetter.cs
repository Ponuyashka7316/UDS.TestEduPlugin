using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDS.GurinPlugin
{


    public class FieldSetter : CodeActivity
    {
        #region Workflow parameters

        [RequiredArgument]
        [Input("TextToAdd")]
        public InArgument<string> TextParam { get; set; }

        [RequiredArgument]
        [Input("IntToAdd")]
        public InArgument<int> IntParam { get; set; }

        #endregion

        protected override void Execute(CodeActivityContext executionContext)
        {
            var context = executionContext.GetExtension<IWorkflowContext>();
            var serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            var service = serviceFactory.CreateOrganizationService(context.UserId);

            var textPar = TextParam.Get<string>(executionContext);
            var intPar = IntParam.Get<int>(executionContext);

            Entity updatedEntity = new Entity(context.PrimaryEntityName);
            updatedEntity.Id = context.PrimaryEntityId;
            updatedEntity["new_decimalfield"] = 555;
            updatedEntity["new_changeprocess"] = intPar;
            service.Update(updatedEntity);
        }

    }
}

