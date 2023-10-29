using Portfolio.Domain.Base.Interfaces.Data;

namespace Portfolio.Domain.RawQueryResponse
{
    public class StringRawQueryResponse : IRawQueryResponse
    {
        public string String { get; set; }
    }
}