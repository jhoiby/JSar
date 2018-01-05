using JSar.Membership.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace JSar.Membership.Messages.Results
{
    /// <inheritdoc />
    public class CommonResult : ICommonResult
    {

        Outcome _outcome;

        public CommonResult(Outcome outcome)
        {
            _outcome = outcome;
            Data = default(string);
        }

        public CommonResult(Outcome outcome, string flashMessage)
        {
            _outcome = outcome;
            TotalResults = 0;
            FlashMessage = flashMessage;
            Data = default(string);
        }
        
        public CommonResult(Outcome outcome, string flashMessage, ResultErrorCollection errors)
        {
            _outcome = outcome;
            FlashMessage = flashMessage;
            Errors = errors;
            TotalResults = errors.Count;
            Data = default(string);
        }

        public CommonResult(Outcome outcome, dynamic data)
        {
            _outcome = outcome;
            FlashMessage = "";
            TotalResults = 1;
            Data = data;
        }

        public CommonResult(Outcome outcome, string flashMessage, dynamic data)
        {
            _outcome = outcome;
            FlashMessage = flashMessage;
            TotalResults = 1;
            Data = data;
        }

        public CommonResult(Outcome outcome, int totalResults, dynamic data)
        {
            _outcome = outcome;
            FlashMessage = "";
            TotalResults = totalResults;
            Data = data;
        }

        public CommonResult(Outcome outcome, string flashMessage, int totalResults, dynamic data)
        {
            _outcome = outcome;
            FlashMessage = flashMessage;
            TotalResults = totalResults;
            Data = data;
        }

        public CommonResult(Outcome outcome, string flashMessage, int totalResults, ResultErrorCollection errors, dynamic data)
        {
            _outcome = outcome;
            FlashMessage = flashMessage;
            TotalResults = totalResults;
            Errors = errors;
            Data = data;
        }

        public CommonResult()
        {
        }

        /// <inheritdoc />
        public dynamic Data { get; }

        /// <inheritdoc />
        public ResultErrorCollection Errors { get; }

        /// <inheritdoc />
        public int TotalResults { get; }

        /// <inheritdoc />
        public string FlashMessage { get; }

        /// <inheritdoc />
        public bool Succeeded { get { return Outcome == Outcome.Succeeded; }  }

        /// <inheritdoc />
        public Outcome Outcome { get { return _outcome; } }
        
        public static implicit operator bool(CommonResult result)
        {
                try { return result.Outcome == Outcome.Succeeded; }
                catch { return false; }
        }
    }
}
