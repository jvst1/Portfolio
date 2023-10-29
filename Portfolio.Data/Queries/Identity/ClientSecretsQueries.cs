using IdentityServer4.EntityFramework.Entities;

namespace Portfolio.Data.Queries.Identity
{
    public static class ClientSecretsQueries
    {
        public class GetAll : QueryBase<ClientSecret>
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
                var sql = @"SELECT * FROM dbo.ClientSecrets (NOLOCK) ";

                if (!string.IsNullOrWhiteSpace(ClientId))
                    AddCondition($"ClientId = {ClientId}");

                if (!string.IsNullOrWhiteSpace(Search))
                    AddCondition(GetSearch(Search, "Description", "Value", "Type"));

                var rawSql = sql + GetWhere();

                return rawSql;
            }
        }
    }
}