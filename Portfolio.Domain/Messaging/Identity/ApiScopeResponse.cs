using IdentityServer4.EntityFramework.Entities;

namespace Portfolio.Domain.Messaging.Identity
{
    public class ApiScopeResponse
    {
        public int Id { get; set; }

        public string Scope { get; set; }

        public int ApiResourceId { get; set; }

        public ApiResource ApiResource { get; set; }
    }
}
