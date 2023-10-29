using IdentityServer4.EntityFramework.Entities;

namespace Portfolio.Domain.Messaging.Identity
{
    public class ApiResourceSecretsRequest : Secret
    {
        public int ApiResourceId { get; set; }

        public ApiResource ApiResource { get; set; }
    }
}
