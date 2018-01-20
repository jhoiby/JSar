using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JSar.Web.UI.Domain.ValueTypes;
using JSar.Web.UI.Extensions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators.Internal;

namespace JSar.Web.UI.Infrastructure.Mail
{
    public class SmtpMessage : ISmtpMessage
    { 
        public SmtpMessage(List<IEmailAddress> to, IEmailAddress from, string subject, string body)
        {
            To = HasNoAddresses(to)
                ? throw new ArgumentException("To field of email must contain at least one email address")
                : to;

            From = from ?? throw new ArgumentException("Email message must have a from address");

            Subject = subject;

            Body = body;
        }

        public SmtpMessage(IEmailAddress to, IEmailAddress from, string subject, string body)
            : this (new List<IEmailAddress> {to}, from, subject, body )
        {
        }

        public List<IEmailAddress> To { get; }
        public IEmailAddress From { get; }
        public string Subject { get; }
        public string Body { get; }
        
        private bool HasNoAddresses(List<IEmailAddress> addresses)
        {
            foreach (IEmailAddress address in addresses)
            {
                if (!address.Address.IsNullOrWhiteSpace())
                    return false;
            }

            return true;
        }
    }
}
