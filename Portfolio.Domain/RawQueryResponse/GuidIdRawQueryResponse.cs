using Portfolio.Domain.Base.Interfaces.Data;

namespace Portfolio.Domain.RawQueryResponse
{
    public class GuidIdRawQueryResponse : IRawQueryResponse
    {
        public Guid Id { get; set; }
    }
}