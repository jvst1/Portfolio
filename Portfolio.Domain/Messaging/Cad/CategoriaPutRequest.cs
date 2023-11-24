namespace Portfolio.Domain.Messaging.Cad
{
    public class CategoriaPutRequest
    {
        public Guid Codigo { get; set; }
        public string ImageUrl { get; set; }
        public string Nome { get; set; }
    }
}
