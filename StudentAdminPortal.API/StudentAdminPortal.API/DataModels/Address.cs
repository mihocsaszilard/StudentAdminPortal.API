using System;

namespace StudentAdminPortal.API.DataModels
{
    public class Address
    {
        public Guid Id { get; set; }
        public string PhysicalAdress { get; set; }
        public string PostalAdress { get; set; }

        //Navigation Property
        public Guid StudentId { get; set; }
    }
}
