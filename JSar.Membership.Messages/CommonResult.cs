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
            FlashMessage = flashMessage;
        }

        public CommonResult(ResultStatus status, int totalResults, dynamic data)
        {
            _status = status;
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

        public dynamic Data { get; }

        public int TotalResults { get; }

        public string FlashMessage { get; }

        public bool Success
        {
            get
            {
                try { return Status == ResultStatus.Success; }
                catch { return false; }
            }
        }

        public ResultStatus Status { get { return _status; } }

        public static implicit operator bool(CommonResult result)
        {
                try { return result.Status == ResultStatus.Success; }
                catch { return false; }
        }
    }
}
