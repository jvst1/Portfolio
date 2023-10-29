using IdentityServer4.EntityFramework.Entities;

namespace Portfolio.Domain.Messaging.Identity
{
    public class ApiResourceSecretsResponse : Secret
    {
        public int ApiResourceId { get; set; }

        public ApiResource ApiResource { get; set; }
    }
}
