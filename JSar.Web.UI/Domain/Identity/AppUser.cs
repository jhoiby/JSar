using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using JSar.Web.UI.Domain.Aggregates;
using JSar.Web.UI.Domain.Events;

namespace JSar.Web.UI.Domain.Identity
{
    // Add profile data for application users by adding properties to the AppUser class
    public class AppUser : IdentityUser<Guid>, IAggregateRoot
    {
        // Events to publish when DbContext.SaveChanges() is called
        protected List<IDomainEvent> _domainEventsQueue = new List<IDomainEvent>();

        // Paramaterless constructor required for Entity Framework
        protected AppUser() { }

        public AppUser(string email)
        {
            base.Email = email;
            base.UserName = email;
        }

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

        #region Implementation for IAggregateRoot
        public List<IDomainEvent> DomainEvents => _domainEventsQueue;

        public void AddDomainEvent(IDomainEvent eventItem)
        {
            _domainEventsQueue = _domainEventsQueue ?? new List<IDomainEvent>();
            _domainEventsQueue.Add(eventItem);
        }

        public void RemoveDomainEvent(IDomainEvent eventItem)
        {
            if (_domainEventsQueue is null) return;
            _domainEventsQueue.Remove(eventItem);
        }
        #endregion
    }
}
