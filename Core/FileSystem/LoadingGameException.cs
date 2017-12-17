using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.FileSystem
{
    class LoadingGameException : Exception
    {
        public LoadingGameException()
        {
        }

        public LoadingGameException(string message) : base(message)
        {
        }

        public LoadingGameException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LoadingGameException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
