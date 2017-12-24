using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    class GameLoadingException : Exception
    {
        public GameLoadingException()
        {
        }

        public GameLoadingException(string message) : base(message)
        {
        }

        public GameLoadingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected GameLoadingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
