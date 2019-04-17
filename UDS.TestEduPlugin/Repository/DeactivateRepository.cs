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


namespace UDS.GurinPlugin.Repository
{
    public class DeactivateRepository
    {
        private IOrganizationService _service;
        private OrganizationServiceProxy _serviceProxy;
        private const string EntityName = "myprefix_gu_main";
        DateTime date1 = DateTime.Now;
        public DeactivateRepository(IOrganizationService service)
        {
            _service = service;
        }

        //Выбираем все сущности А где поле лукапа не пустое и есть связанные Б где поле лукапа не пустое
        public EntityCollection GetRecords()
        {
            var query = new QueryExpression(EntityName)
            {
                Distinct = true,

                ColumnSet = new ColumnSet(true),
                Criteria = new FilterExpression(LogicalOperator.And)
                {
                    Conditions =
                                 {
                                    new ConditionExpression("new_lookupfield", ConditionOperator.NotNull)
                                   // new ConditionExpression("createdon", ConditionOperator.AboveOrEqual, DateTime.Now.AddDays(-2).ToString("s"))
                                 }
                },
                LinkEntities =
                {
                    new LinkEntity(EntityName, "new_new_l_myprefix_gu_main", "myprefix_gu_mainid", "myprefix_gu_mainid", JoinOperator.Inner)
                        // делаем линк на промежуточную таблицу
                  {
                      LinkEntities =
                      {
                      new LinkEntity("new_new_l_myprefix_gu_main", "new_l", "new_lid", "new_lid",
                                JoinOperator.Inner)
                            {
                              LinkCriteria = new FilterExpression(LogicalOperator.And)
                              {
                                    Conditions =
                                 {
                                    new ConditionExpression("new_accountid", ConditionOperator.NotNull)
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
                var recordToDeactivate =
                    GetRecordsTask5(item.GetAttributeValue<EntityReference>("new_lookupfield"), item.Id);
                if (recordToDeactivate != null)
                {
                    //deactivate

                    recordToDeactivate["statecode"] = new OptionSetValue(1);
                    recordToDeactivate["statuscode"] = new OptionSetValue(2);
                    _service.Update(recordToDeactivate);
                }
            }
            return res;
        }

        //по переданному лукапу делаем запрос NN на выборку всех  записей сущности А, где лукап записи А такой же как и лукап записи Б 
        public Entity GetRecordsTask5(EntityReference id, Guid mainId)
        {
            var query = new QueryExpression("myprefix_gu_main")
            {
                Distinct = true,

                ColumnSet = new ColumnSet(true),
                Criteria = new FilterExpression(LogicalOperator.And)
                {
                    Conditions =
                                 {
                                   new ConditionExpression("new_lookupfield", ConditionOperator.Equal, id.Id),
                                   new ConditionExpression("myprefix_gu_mainid", ConditionOperator.Equal, mainId)
                                 }
                },
                LinkEntities =
                {
                    new LinkEntity("myprefix_gu_main", "new_new_l_myprefix_gu_main", "myprefix_gu_mainid", "myprefix_gu_mainid", JoinOperator.Inner)
                        // делаем линк на промежуточную таблицу
                  {
                      LinkEntities =
                      {
                      new LinkEntity("new_new_l_myprefix_gu_main", "new_l", "new_lid", "new_lid",
                                JoinOperator.Inner)
                            {
                              LinkCriteria = new FilterExpression(LogicalOperator.And)
                              {
                                    Conditions =
                                 {
                                      new ConditionExpression("new_accountid", ConditionOperator.Equal, id.Id)
                                 }
                              }
                            }
                        }

                    }



                }
            };
            EntityCollection mainEntities = _service.RetrieveMultiple(query);

            if (mainEntities.Entities.Count > 0)
            {
                return mainEntities[0];
            }
            return null;
        }

    }
}
