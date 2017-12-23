using System;
using System.Collections.Generic;
using System.Text;

namespace JSar.Membership.Messages
{
    /// <summary>
    /// Provides a common method for returning results of commands and queries. Can be
    /// used by commands to return success results and error details, and by queries
    /// to return a result set. 
    /// </summary>
    public interface ICommonResult
    {
        /// <summary>
        /// Objects resulting from a query, or error messages from a command. If command error
        /// result, will contain a List<string> object with one or more error messages. If query
        /// result, object types contained will vary based on query.
        /// </summary>
        dynamic Data { get; }

        /// <summary>
        /// A count of the objects contained in Data
        /// </summary>
        int TotalResults { get; }

        /// <summary>
        /// A one line summary message to accompany the result.
        /// </summary>
        string FlashMessage { get; }
        
        ResultStatus Status { get; }

        /// <summary>
        /// True if command/query completed without error. (ResultStatus.Succeeded)
        /// </summary>
        bool Success { get; }
    }
}

// Thank you to Daniel Whittaker for this CommonResult pattern:
// http://danielwhittaker.me/2016/04/20/how-to-validate-commands-in-a-cqrs-application/
