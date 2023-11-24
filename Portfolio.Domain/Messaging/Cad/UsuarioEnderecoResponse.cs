using Portfolio.Domain.Base.Interfaces.Data;

namespace Portfolio.Domain.Messaging.Cad
{
    public class UsuarioEnderecoResponse : IRawQueryResponse
    {
        public Guid Codigo { get; set; }
        public string CEP { get; set; }
        public string Logradouro { get; set; }
        public string NroLogradouro { get; set; }
        public string Bairro { get; set; }
        public string Complemento { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
        public DateTime? EnderecoDesde { get; set; }
        public bool EnderecoPrincipal { get; set; }
    }
}
