using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation.Results;

namespace JSar.Membership.Messages.Validators
{
    public static class ValidationExtensions
    {
        public static ResultErrorCollection ToResultErrorCollection(this List<ValidationFailure> failures)
        {
            ResultErrorCollection collection = new ResultErrorCollection();
            foreach (ValidationFailure failure in failures)
            {
                collection.Add(failure.PropertyName,failure.ErrorMessage);
            }

            return collection;
        }
    }
}
