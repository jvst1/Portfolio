using Portfolio.Domain.Base.Interfaces.Data;

namespace Portfolio.Domain.RawQueryResponse
{
    public class IntRawQueryResponse : IRawQueryResponse
    {
        public int Response { get; set; }
    }
}