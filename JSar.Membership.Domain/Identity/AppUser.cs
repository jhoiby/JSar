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
        // Paramaterless constructor required for Entity Framework
        protected AppUser() { }

        public AppUser(string email, string firstName, string lastName, string primaryPhone)
        {
            base.Email = email;
            base.PhoneNumber = primaryPhone;
            base.UserName = email;
            FirstName = firstName;
            LastName = lastName;
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName {
            get { return FirstName + " " + LastName; }
        }

    }
}
