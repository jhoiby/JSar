using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using JSar.Membership.Domain.Aggregates;

namespace JSar.Membership.Domain.Identity
{
    // Add profile data for application users by adding properties to the AppUser class
    public class AppUser : IdentityUser<Guid>, IAggregateRoot
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string WorkPhone { get; set; }
        public string MobilePhone { get; set; }
        public string OtherPhone { get; set; }
        public bool RequirePasswordChange { get; set; }
        public string ManagersNotes { get; set; }

    }
}
