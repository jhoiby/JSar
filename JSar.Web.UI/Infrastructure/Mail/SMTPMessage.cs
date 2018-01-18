using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JSar.Web.UI.Extensions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators.Internal;
using Microsoft.Graph;

namespace JSar.Web.UI.Infrastructure.Mail
{
    public class SmtpMessage : IMailMessage
    {
        public SmtpMessage(string[] to, string from, string subject, string body)
        {
            To = HasNoAddresses(to)
                ? throw new ArgumentException("To field of email must contain at least one email address")
                : to;

            From = from.IsNullOrWhiteSpace()
                ? throw new ArgumentException("From field of email cannot be empty", nameof(from))
                : from.Trim();

            Subject = subject.Trim();

            Body = body.Trim();
        }

        public SmtpMessage(string to, string from, string subject, string body)
            : this (new string[] {to}, from, subject, body )
        {
        }

        public string[] To { get; }
        public string From { get; }
        public string Subject { get; }
        public string Body { get; }
        
        private bool HasNoAddresses(string[] addresses)
        {
            foreach (string address in addresses)
            {
                if (!address.IsNullOrWhiteSpace())
                    return false;
            }

            return true;
        }
    }
}
