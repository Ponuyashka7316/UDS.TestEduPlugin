using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using System.ServiceModel;
using System.ServiceModel.Description;
using CrmEarlyBound;

namespace UDS.GurinPlugin.Repository
{
    class DeactivateWithLinqRepository
    {
        private IOrganizationService _service;
        private OrganizationServiceProxy _serviceProxy;
        private const string EntityName = "myprefix_gu_main";
        Uri oUri = new Uri("https://education.crm2016.uds.systems/XRMServices/2011/Organization.svc");
        ClientCredentials clientCredentials = new ClientCredentials();

        public DeactivateWithLinqRepository(IOrganizationService service)
        {
            _service = service;
            clientCredentials.UserName.UserName = "UDS\\s.gurin";
            clientCredentials.UserName.Password = "SGuds3179";

        }
        public IEnumerable<myprefix_gu_main> GetRecords()
        {

            OrganizationServiceProxy _serviceProxy = new OrganizationServiceProxy(oUri, null, clientCredentials, null);
            _serviceProxy.EnableProxyTypes();

            var _context = new CrmServiceContext(_serviceProxy);
            var guMains = (
                    from a in _context.myprefix_gu_mainSet
                    join relationEntity in _context.new_new_l_myprefix_gu_mainSet on a.myprefix_gu_mainId
                            equals relationEntity.myprefix_gu_mainid
                    join L in _context.new_lSet on relationEntity.new_lid equals L.new_lId
                    where a.new_lookupfield != null && L.new_accountid != null
                    select a).ToList();

            foreach (var item in guMains)
            {
                var recordToDeactivate = GetRecordsTask5(item.GetAttributeValue<EntityReference>("new_lookupfield"), item.Id);
                if (recordToDeactivate != null)
                {
                    Entity tempEntity = new Entity(EntityName, recordToDeactivate.Id);

                    //deactivate
                    tempEntity["statecode"] = new OptionSetValue(1);
                    tempEntity["statuscode"] = new OptionSetValue(2);
                    _service.Update((Entity)tempEntity);
                }
            }
            return guMains.Distinct();

        }

        public Entity GetRecordsTask5(EntityReference id, Guid guId)
        {
            OrganizationServiceProxy _serviceProxy = new OrganizationServiceProxy(oUri, null, clientCredentials, null);
            _serviceProxy.EnableProxyTypes();

            var _context = new CrmServiceContext(_serviceProxy);
            var guMains = (
                    from a in _context.myprefix_gu_mainSet
                    join relationEntity in _context.new_new_l_myprefix_gu_mainSet on a.myprefix_gu_mainId equals relationEntity.myprefix_gu_mainid
                    join L in _context.new_lSet on relationEntity.new_lid equals L.new_lId
                    where L.new_accountid == id && a.new_lookupfield == id && a.myprefix_gu_mainId == guId
                    select a).ToList();
            if (guMains.Count > 0)
            {
                return guMains.Distinct().FirstOrDefault();
            }
            return null;
        }
    }
}
