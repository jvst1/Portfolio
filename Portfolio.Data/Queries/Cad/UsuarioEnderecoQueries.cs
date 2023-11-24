using Portfolio.Domain.Entities.Cad;

namespace Portfolio.Data.Queries.Cad
{
    public static class UsuarioEnderecoQueries
    {
        public class GetAll : QueryBase<UsuarioEndereco>
        {
            public string Search { get; }
            public Guid CodigoUsuario { get; }
            public bool? EnderecoPrincipal { get; }

            public GetAll(string search, Guid codigoUsuario, bool? enderecoPrincipal)
            {
                Search = search;
                CodigoUsuario = codigoUsuario;
                EnderecoPrincipal = enderecoPrincipal;
            }

            public override string GetSql()
            {
                var sql = @"SELECT * FROM cad.UsuarioEndereco (NOLOCK)";

                AddCondition($"CodigoUsuario = @{nameof(CodigoUsuario)}");

                if (!string.IsNullOrEmpty(Search))
                    AddCondition(GetSearch(Search, "CEP", "Logradouro", "Bairro", "Complemento", "Cidade"));

                if (EnderecoPrincipal.HasValue)
                    AddCondition($"EnderecoPrincipal = @{nameof(EnderecoPrincipal)}");

                return sql + GetWhere();
            }
        }
    }
}
