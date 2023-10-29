using Portfolio.Domain.Base.Interfaces.Data;

namespace Portfolio.Domain.RawQueryResponse
{
    public class GuidStringRawQueryResponse : IRawQueryResponse
    {
        public Guid Value { get; set; }
        public string Text { get; set; }
    }
}
