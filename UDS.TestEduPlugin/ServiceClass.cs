using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using UDS.GurinPlugin.Repository;

namespace UDS.GurinPlugin
{
    public class ServiceClass
    {
        private IOrganizationService _service;
        public ServiceClass(IOrganizationService service)
        {
            _service = service;
        }

        //uncomment nessecary code
        public void MainMethod()
        {
            DeactivateWithLinqRepository rep = new DeactivateWithLinqRepository(_service);

            var GuMains1 = rep.GetRecords();
            if (GuMains1 != null)
            {
                Console.WriteLine(GuMains1.Count());
            }
            else
            {
                Console.WriteLine("No Gu mains");
            }
            Console.ReadLine();
            //ContactRepository contactRepository = new ContactRepository(_service);
            //EntityCollection contacts = contactRepository.GetContactsWithPhone(new Guid("09C2DEE9-84B8-E711-80FB-00155D05FA01"));
            //if (contacts != null)
            //{
            //    Console.WriteLine(contacts.Entities.Count);
            //}
            //else
            //{
            //    Console.WriteLine("No contacts");
            //}

            /*
           DeactivateRepository rep = new DeactivateRepository(_service);

           EntityCollection GuMains = rep.GetRecords();
           if (GuMains != null)
           {
               Console.WriteLine(GuMains.Entities.Count);
           }
           else
           {
               Console.WriteLine("No Gu mains");
           }
           Console.ReadLine();
           */
            // DeactivateRelatedRepository deact = new DeactivateRelatedRepository(_service);
            // deact.GetRecords(Guid.Parse("D8CB3443-1252-E911-8117-00155D05FA01"));



        }
    }
}
