using JSar.Membership.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace JSar.Membership.Messages
{
    /// <inheritdoc />
    public class CommonResult : ICommonResult
    {
        ResultOutcome _outcome;

        public CommonResult(ResultOutcome outcome)
        {
            _outcome = outcome;
            Data = default(string);
        }

        public CommonResult(ResultOutcome outcome, string flashMessage)
        {
            _outcome = outcome;
            TotalResults = 0;
            FlashMessage = flashMessage;
            Data = default(string);
        }
        
        public CommonResult(ResultOutcome outcome, string flashMessage, ResultErrorCollection errors)
        {
            _outcome = outcome;
            FlashMessage = flashMessage;
            Errors = errors;
            TotalResults = errors.Count;
            Data = default(string);
        }

        public CommonResult(ResultOutcome outcome, dynamic data)
        {
            _outcome = outcome;
            FlashMessage = "";
            TotalResults = 1;
            Data = data;
        }

        public CommonResult(ResultOutcome outcome, string flashMessage, dynamic data)
        {
            _outcome = outcome;
            FlashMessage = flashMessage;
            TotalResults = 1;
            Data = data;
        }

        public CommonResult(ResultOutcome outcome, int totalResults, dynamic data)
        {
            _outcome = outcome;
            FlashMessage = "";
            TotalResults = totalResults;
            Data = data;
        }

        public CommonResult(ResultOutcome outcome, string flashMessage, int totalResults, dynamic data)
        {
            _outcome = outcome;
            FlashMessage = flashMessage;
            TotalResults = totalResults;
            Data = data;
        }

        public CommonResult(ResultOutcome outcome, string flashMessage, int totalResults, ResultErrorCollection errors, dynamic data)
        {
            _outcome = outcome;
            FlashMessage = flashMessage;
            TotalResults = totalResults;
            Errors = errors;
            Data = data;
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
        public bool Succeeded { get { return Outcome == ResultOutcome.Succeeded; }  }

        /// <inheritdoc />
        public ResultOutcome Outcome { get { return _outcome; } }
        
        public static implicit operator bool(CommonResult result)
        {
                try { return result.Outcome == ResultOutcome.Succeeded; }
                catch { return false; }
        }
    }
}
