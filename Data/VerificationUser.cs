using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace UserRegistration.Data
{
    public class VerificationUser: IdentityUser
    {
        public string VerificationCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public bool IsPromotionsEnable { get; set; }
        public bool IsUpdated { get; set; }

    }
}
