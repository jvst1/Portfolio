namespace Portfolio.Domain.Messaging.Identity
{
    public class ClientScopesRequest
    {
        public int ClientId { get; set; }
        public string Scopes { get; set; }
    }
}
