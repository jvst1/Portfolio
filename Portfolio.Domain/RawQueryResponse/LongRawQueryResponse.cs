using Portfolio.Domain.Base.Interfaces.Data;

namespace Portfolio.Domain.RawQueryResponse
{
    public class LongRawQueryResponse : IRawQueryResponse
    {
        public long Response { get; set; }
    }
}