using Portfolio.Infrastructure.Attributes;

namespace Portfolio.Domain.Messaging.Identity
{
    public class ApiResourceRequest
    {
        public int Id { get; set; }

        public bool Enabled { get; set; }

        [Truncate(200)]
        public string Name { get; set; }

        [Truncate(200)]
        public string DisplayName { get; set; }

        [Truncate(1000)]
        public string Description { get; set; }

        [Truncate(100)]
        public string AllowedAccessTokenSigningAlgorithms { get; set; }

        public bool ShowInDiscoveryDocument { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Updated { get; set; }

        public DateTime? LastAccessed { get; set; }

        public bool NonEditable { get; set; }
    }
}
