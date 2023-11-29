using System.Runtime.Serialization;

namespace Portfolio.Infrastructure.Exceptions
{
    public class UserInactiveException : Exception
    {
        public UserInactiveException()
        {
        }

        public UserInactiveException(string message) : base(message)
        {
        }

        public UserInactiveException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UserInactiveException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
