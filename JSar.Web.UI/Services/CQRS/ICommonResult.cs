using System;
using System.Collections.Generic;
using System.Text;

namespace JSar.Web.UI.Services.CQRS
{
    /// <summary>
    /// Provides a common method for returning results of commands and queries. Used to return
    /// command and query execution status (see ResultStatus) and, from queries, the resulting
    /// data.
    /// </summary>
    public interface ICommonResult
    {
        /// <summary>
        /// Objects returned from a query, with the object type varying based on the query.
        /// The type of the Data object returned is defined in the query comments. 
        /// 
        /// In rare circumstances a command may return additional result data. For example an
        /// Asp.Net Identity SignInByPassword will return a SignInResult object. 
        /// 
        /// To maintain CQRS principles it is recommended to not return entities or other
        /// command-generated artifacts that should instead be returned with a query.
        /// </summary>
        dynamic Data { get; }

        /// <summary>
        /// Returns a ResultErrorDictionary with the list of errors.
        /// </summary>
        ResultErrorCollection Errors { get; }

        /// <summary>
        /// A count of results or, if Status != Success, a count of errors in the
        /// ResultErrorCollection
        /// </summary>
        int TotalResults { get; }

        /// <summary>
        /// A one line summary message to accompany the result.
        /// </summary>
        string FlashMessage { get; }
        
        Outcome Outcome { get; }

        /// <summary>
        /// True if command/query completed without error (ResultOutcome.Succeeded). The
        /// should also be returned as an implicit bool operation on ICommonResult, although
        /// this cannot be required by the interface.
        /// </summary>
        bool Succeeded { get; }
    }
}

// Thank you to Daniel Whittaker for this CommonResult pattern:
// http://danielwhittaker.me/2016/04/20/how-to-validate-commands-in-a-cqrs-application/
