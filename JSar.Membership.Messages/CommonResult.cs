using JSar.Membership.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace JSar.Membership.Messages
{
    /// <inheritdoc />
    public class CommonResult : ICommonResult
    {
        ResultStatus _status;

        public CommonResult(ResultStatus status)
        {
            _status = status;
        }

        public CommonResult(ResultStatus status, string flashMessage)
        {
            _status = status;
            TotalResults = 0;
            FlashMessage = flashMessage;
            Data = default(string);
        }
        
        public CommonResult(ResultStatus status, string flashMessage, ResultErrorCollection errors)
        {
            _status = status;
            FlashMessage = flashMessage;
            Errors = errors;
            TotalResults = errors.Count;
        }

        public CommonResult(ResultStatus status, string flashMessage, dynamic data)
        {
            _status = status;
            FlashMessage = flashMessage;
            TotalResults = 1;
            Data = data;
        }

        public CommonResult(ResultStatus status, int totalResults, dynamic data)
        {
            _status = status;
            FlashMessage = "";
            TotalResults = totalResults;
            Data = data;
        }

        public CommonResult(ResultStatus status, string flashMessage, int totalResults, dynamic data)
        {
            _status = status;
            FlashMessage = flashMessage;
            TotalResults = totalResults;
            Data = data;
        }

        public CommonResult(ResultStatus status, string flashMessage, int totalResults, ResultErrorCollection errors, dynamic data)
        {
            _status = status;
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
        public bool Success { get { return Status == ResultStatus.Success; }  }

        /// <inheritdoc />
        public ResultStatus Status { get { return _status; } }
        
        public static implicit operator bool(CommonResult result)
        {
                try { return result.Status == ResultStatus.Success; }
                catch { return false; }
        }
    }
}
