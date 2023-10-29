using System;

namespace Portfolio.Domain.Messaging.Identity
{
    public class ClientSecretRequest
    {
        public int id { get; set; }
        public string Description { get; set; }
        public DateTime? Expiration { get; set; }
        public int ClientId { get; set; }
        public string ChavePEM { get; set; }

    }
}
