using IdentityServer4.EntityFramework.Entities;

namespace Portfolio.Data.Queries.Identity
{
    public static class ApiResourceSecretsQueries
    {
        public class GetAll : QueryBase<ApiResourceSecret>
        {
            public GetAll(string search, string idResource)
            {
                Search = search;
                IdResource = idResource;
            }

            public string Search { get; }
            public string IdResource { get; }

            public override string GetSql()
            {
                var sql = @"SELECT * FROM dbo.ApiResourceSecrets (NOLOCK)";

                if (!string.IsNullOrWhiteSpace(IdResource))
                    AddCondition($"ApiResourceId = {IdResource}");

                if (!string.IsNullOrWhiteSpace(Search))
                    AddCondition(GetSearch(Search, "Description", "Value"));

                var rawSql = sql + GetWhere();

                return rawSql;
            }
        }
    }
}