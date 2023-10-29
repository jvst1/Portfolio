using Newtonsoft.Json;
using Portfolio.Domain.Base.Interfaces.Data;

namespace Portfolio.Domain.RawQueryResponse
{
    public class RawQueryResponse : IRawQueryResponse
    {
        [JsonProperty("sucesso")]
        public bool Sucesso;
    }
}
