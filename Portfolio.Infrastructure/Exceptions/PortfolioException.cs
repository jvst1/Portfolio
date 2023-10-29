using System.Runtime.Serialization;

namespace Portfolio.Infrastructure.Exceptions
{
    public class PortfolioException : Exception
    {
        public PortfolioException()
        {
        }

        protected PortfolioException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public PortfolioException(string message) : base(message)
        {
        }

        public PortfolioException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
