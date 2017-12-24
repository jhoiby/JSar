using System;
using System.Collections.Generic;
using System.Text;

namespace JSar.Membership.Messages.Commands
{
    public class RegisterLocalUser : Command<CommonResult>
    {
        public RegisterLocalUser(
            string FirstName, 
            string LastName, 
            string PrimaryPhone, 
            string Email, 
            string Password, 
            Guid messageId = default(Guid)) 
            : base (messageId)
        {
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.PrimaryPhone = PrimaryPhone;
            this.Email = Email;
            this.Password = Password;
        }

        public string FirstName { get; }
        public string LastName { get; }
        public string PrimaryPhone { get; }
        public string Email { get; }
        public string Password { get; }
    }
}
