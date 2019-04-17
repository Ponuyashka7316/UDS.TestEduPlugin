using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace UDS.GurinPlugin.Repository
{
    public class ContactRepository
    {
        private IOrganizationService _service;
        private const string EntityName = "contact";
        public ContactRepository(IOrganizationService service)
        {
            _service = service;
        }

        public EntityCollection GetContactsWithPhone(Guid accId)
        {
            QueryExpression qe = new QueryExpression(EntityName)
            {
                ColumnSet = new ColumnSet("fullname"),
                Criteria = new FilterExpression()
                {
                  Conditions  =
                  {
                      new ConditionExpression("parentcustomerid",ConditionOperator.Equal,accId) 
                  }
                },
            };
            EntityCollection contacts = _service.RetrieveMultiple(qe);
            if (contacts.Entities.Count > 0)
            {
                return contacts;
            }
            return null;
        }
    }
}
