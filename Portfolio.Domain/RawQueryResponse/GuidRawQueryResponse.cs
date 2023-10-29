using Portfolio.Domain.Base.Interfaces.Data;

namespace Portfolio.Domain.RawQueryResponse
{
    public class GuidRawQueryResponse : IRawQueryResponse
    {
        public Guid Guid { get; set; }
    }
}