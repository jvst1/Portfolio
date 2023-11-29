using System.Runtime.Serialization;

namespace Portfolio.Infrastructure.Exceptions
{
    public class UserAlreadyActivatedException : Exception
    {
        public UserAlreadyActivatedException()
        {
        }

        public UserAlreadyActivatedException(string message) : base(message)
        {
        }

        public UserAlreadyActivatedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UserAlreadyActivatedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
