using System;
using System.Collections.Generic;
using FluentValidation.Results;
using JSar.Membership.Services.CQRS;

namespace JSar.Membership.Services.Validation
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
