using System;

namespace JSar.Web.UI.Services.CQRS
{
    /// <inheritdoc />
    public class CommonResult : ICommonResult
    {

        Outcome _outcome;

        public CommonResult(Guid messageId, Outcome outcome)
        {
            MessageId = messageId;
            _outcome = outcome;
            Data = default(string);
        }

        public CommonResult(Guid messageId, Outcome outcome, string flashMessage)
        {
            MessageId = messageId;
            _outcome = outcome;
            TotalResults = 0;
            FlashMessage = flashMessage;
            Data = default(string);
        }
        
        public CommonResult(Guid messageId, Outcome outcome, string flashMessage, ResultErrorCollection errors)
        {
            MessageId = messageId;
            _outcome = outcome;
            FlashMessage = flashMessage;
            Errors = errors;
            TotalResults = errors.Count;
            Data = default(string);
        }

        public CommonResult(Guid messageId, Outcome outcome, dynamic data)
        {
            MessageId = messageId;
            _outcome = outcome;
            FlashMessage = string.Empty;
            TotalResults = 1;
            Data = data;
        }

        public CommonResult(Guid messageId, Outcome outcome, string flashMessage, dynamic data)
        {
            MessageId = messageId;
            _outcome = outcome;
            FlashMessage = flashMessage;
            TotalResults = 1;
            Data = data;
        }

        public CommonResult(Guid messageId, Outcome outcome, int totalResults, dynamic data)
        {
            MessageId = messageId;
            _outcome = outcome;
            FlashMessage = string.Empty;
            TotalResults = totalResults;
            Data = data;
        }

        public CommonResult(Guid messageId, Outcome outcome, string flashMessage, int totalResults, dynamic data)
        {
            MessageId = messageId;
            _outcome = outcome;
            FlashMessage = flashMessage;
            TotalResults = totalResults;
            Data = data;
        }

        public CommonResult(Guid messageId, Outcome outcome, string flashMessage, int totalResults, ResultErrorCollection errors, dynamic data)
        {
            MessageId = messageId;
            _outcome = outcome;
            FlashMessage = flashMessage;
            TotalResults = totalResults;
            Errors = errors;
            Data = data;
        }

        // TODO: Why is this empty constructor here?
        //public CommonResult()
        //{
        //}

        public Guid MessageId { get; }

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
