using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDS.GurinPlugin.Repository
{
    public class DeactivateRelatedRepository
    {
        private IOrganizationService _service;
        private OrganizationServiceProxy _serviceProxy;
        private const string EntityName = "myprefix_gu_main";
        public DeactivateRelatedRepository(IOrganizationService service)
        {
            _service = service;

        }

        public void GetRecords(Guid id)
        {
            var query = new QueryExpression("new_l")
            {
                Distinct = true,

                ColumnSet = new ColumnSet(true),
                LinkEntities =
                {
                    new LinkEntity("new_l", "new_new_l_myprefix_gu_main", "new_lid", "new_lid", JoinOperator.Inner)
                  {
                      LinkEntities =
                      {
                      new LinkEntity("new_new_l_myprefix_gu_main", "myprefix_gu_main", "myprefix_gu_mainid", "myprefix_gu_mainid",
                                JoinOperator.Inner)
                            {
                              LinkCriteria = new FilterExpression(LogicalOperator.And)
                              {
                                    Conditions =
                                 {
                                    new ConditionExpression("myprefix_gu_mainid", ConditionOperator.Equal, id)
                                 }
                              }
                            }
                        }
                    }
                }
            };
            var res = _service.RetrieveMultiple(query);    // .Entities;
            foreach (var item in res.Entities)
            {
                SetStateRequest setStateRequest = new SetStateRequest()
                {
                    EntityMoniker = new EntityReference
                    {
                        Id = item.Id,
                        LogicalName = item.LogicalName,
                    },
                    State = new OptionSetValue(1),
                    Status = new OptionSetValue(2)
                };
                _service.Execute(setStateRequest);
            }
        }
    }
}
