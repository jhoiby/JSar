using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.Serialization;
using System.Text;

namespace JSar.Membership.Messages.Results
{
    public class ResultErrorCollection : NameValueCollection
    {
        /// <summary>
        /// A collection of errors that occured during handling of a command or query. The name
        /// contains the name of the parameter, if any, that triggered the error. The value
        /// contains the error message. If the error message is not related to a specific paramater
        /// or other identifiable item specific to the command/query the name may be set to "".
        /// Multiple identical names are allowed. Extends the NameValueCollection class, except
        /// for constructors of NameValueCollection that had hash and comparer parameters marked
        /// as "obsolete".
        /// </summary>
        public ResultErrorCollection() : base()
        {
        }

        public ResultErrorCollection(string name, string value) : base()
        {
            this.Add(name, value);
        }

        public ResultErrorCollection(int capacity) : base(capacity)
        {
        }

        public ResultErrorCollection(NameValueCollection collection) : base(collection)
        {
        }

        public ResultErrorCollection(IEqualityComparer comparer) : base(comparer)
        {
        }

        public ResultErrorCollection(int capacity, IEqualityComparer comparer) 
            : base(capacity, comparer)
        {
        }

        public ResultErrorCollection(int capacity, NameValueCollection collection) 
            : base(capacity, collection)
        {
        }

        public ResultErrorCollection(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}
