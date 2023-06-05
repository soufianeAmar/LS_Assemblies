using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace LSAdmin
{
    public class CancelSavingException : System.Exception
    {
        public CancelSavingException()
        {
            
        }
        public CancelSavingException(string message)
            : base(message)
        {
            
        }
        public CancelSavingException(string message, Exception innerException)
            : base(message, innerException)
        {
            
        }
        protected CancelSavingException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
            
        }

    }
}
