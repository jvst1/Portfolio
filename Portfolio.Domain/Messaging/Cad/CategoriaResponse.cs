using Portfolio.Domain.Base.Interfaces.Data;

namespace Portfolio.Domain.Messaging.Cad
{
    public class CategoriaResponse : IRawQueryResponse
    {
        public Guid Codigo { get; set; }
        public string ImageUrl { get; set; }
        public string Nome { get; set; }
        public DateTime? DtInclusao { get; set; }
        public DateTime? DtAlteracao { get; set; }
    }
}
