using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    class LoadingArrangementException : Exception
    {
        public LoadingArrangementException()
        {
        }

        public LoadingArrangementException(string message) : base(message)
        {
        }

        public LoadingArrangementException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LoadingArrangementException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
