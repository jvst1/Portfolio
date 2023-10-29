using Portfolio.Domain.Entities.Cad;
using Portfolio.Domain.RawQueryResponse;
using Portfolio.Infrastructure.Enums;

namespace Portfolio.Data.Queries.Cad
{
    public static class UsuarioQueries
    {
        public class GetAll : QueryBase<Usuario>
        {
            public string[] Situacao { get; }
            public string Search { get; }

            public GetAll(string search, string[] situacao)
            {
                Search = search;
                Situacao = situacao;
            }

            public override string GetSql()
            {
                var sql = @"SELECT * FROM cad.Usuario (NOLOCK)";

                if (Situacao != null && Situacao.Any())
                    AddCondition("Situacao IN (" + string.Join(", ", Situacao) + " )");

                if (!string.IsNullOrEmpty(Search))
                    AddCondition(GetSearch(Search, "Email", "Identificador", "TelefoneCelular"));

                return sql + GetWhere();
            }
        }

        public class GetToSelect : QueryBase<GuidStringRawQueryResponse>
        {
            public SituacaoUsuario SituacaoUsuario => SituacaoUsuario.Ativo;

            public override string GetSql()
            {
                var sql = "SELECT Codigo 'Value', Identificador 'Text' FROM cad.Usuario (NOLOCK)";

                AddCondition($"Situacao = @{nameof(SituacaoUsuario)}");

                return sql + GetWhere();
            }
        }
        public class GetFirstByClientId : QueryBase<Usuario>
        {
            public string ClientId { get; }

            public GetFirstByClientId(string clientId)
            {
                ClientId = clientId;
            }

            public override string GetSql()
            {
                var sql = "SELECT TOP 1 * FROM cad.Usuario (NOLOCK)";

                AddCondition($"ClientIdIdentityServer = @{nameof(ClientId)}");

                return sql + GetWhere();
            }
        }

        public class GetFirstByRefreshToken : QueryBase<Usuario>
        {
            public string RefreshToken { get; }

            public GetFirstByRefreshToken(string refreshToken)
            {
                RefreshToken = refreshToken;
            }

            public override string GetSql()
            {
                var sql = "SELECT TOP 1 * FROM cad.Usuario (NOLOCK)";

                AddCondition($"RefreshToken = @{nameof(RefreshToken)}");

                return sql + GetWhere();
            }
        }

        public class GetFirstByEmail : QueryBase<Usuario>
        {
            public string Email { get; }

            public GetFirstByEmail(string email)
            {
                Email = email;
            }

            public override string GetSql()
            {
                var sql = "SELECT TOP 1 * FROM cad.Usuario (NOLOCK)";

                AddCondition($"Email = @{nameof(Email)}");

                return sql + GetWhere();
            }
        }

        public class ClientIdCadastrado : QueryBase<Usuario>
        {
            public Guid CodigoUsuario { get; }

            public string ClientIdIdentityServer { get; }

            public ClientIdCadastrado(Guid codigo, string clientIdIdentityServer)
            {
                CodigoUsuario = codigo;
                ClientIdIdentityServer = clientIdIdentityServer;
            }

            public override string GetSql()
            {
                var sql = @"SELECT * 
                            FROM cad.Usuario (nolock)";

                AddCondition($"Codigo = @{nameof(CodigoUsuario)}");
                AddCondition($"ClientIdIdentityServer IS NOT NULL");
                AddCondition($"ClientIdIdentityServer != @{nameof(ClientIdIdentityServer)}");

                return sql + GetWhere();
            }
        }

        public class GetByIdentificador : QueryBase<Usuario>
        {
            public string Identificador { get; }


            public GetByIdentificador(string identificador)
            {
                Identificador = identificador;
            }

            public override string GetSql()
            {
                var sql = @"SELECT * 
                            FROM cad.Usuario (NOLOCK)";

                AddCondition($"Identificador = @{nameof(Identificador)}");

                return sql + GetWhere();
            }
        }
    }
}
