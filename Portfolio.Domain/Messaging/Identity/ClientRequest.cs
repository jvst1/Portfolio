using System;

namespace Portfolio.Domain.Messaging.Identity
{
    public class ClientRequest
    {
        public int Id { get; set; }
        public string ClientName { get; set; }
        public string ClientId { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public Guid CodigoUsuario { get; set; }
    }
}
