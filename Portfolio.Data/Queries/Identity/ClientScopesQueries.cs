using IdentityServer4.EntityFramework.Entities;

namespace Portfolio.Data.Queries.Identity
{
    public static class ClientScopesQueries
    {
        public class GetAll : QueryBase<ClientScope>
        {
            public GetAll(string search, string clientId)
            {
                Search = search;
                ClientId = clientId;
            }

            public string Search { get; }
            public string ClientId { get; }

            public override string GetSql()
            {
                var sql = @"SELECT * FROM dbo.ClientScopes AS CLSC (NOLOCK)";

                if (!string.IsNullOrWhiteSpace(ClientId))
                    AddCondition($"ClientId = {ClientId}");

                if (!string.IsNullOrWhiteSpace(Search))
                    AddCondition(GetSearch(Search, "Scope"));

                var rawQuery = sql + GetWhere();

                return rawQuery;
            }
        }
    }
}