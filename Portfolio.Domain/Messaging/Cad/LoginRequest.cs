using Newtonsoft.Json;
using Portfolio.Domain.Base;

namespace Portfolio.Domain.Messaging.Cad
{
    public class LoginRequest : PorfolioRequestBase
    {
        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("senha")]
        public string Senha { get; set; }
    }
}
