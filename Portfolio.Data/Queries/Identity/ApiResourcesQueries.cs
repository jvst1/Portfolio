using IdentityServer4.EntityFramework.Entities;

namespace Portfolio.Data.Queries.Identity
{
    public static class ApiResourcesQueries
    {
        public class GetAll : QueryBase<ApiResource>
        {
            public GetAll(string search)
            {
                Search = search;
            }

            public string Search { get; }

            public override string GetSql()
            {
                var sql = @"SELECT * from dbo.ApiResources (NOLOCK)";

                if (!string.IsNullOrWhiteSpace(Search))
                    AddCondition(GetSearch(Search, "Name", "DisplayName"));

                var rawSql = sql + GetWhere();

                return rawSql;
            }
        }
    }
}