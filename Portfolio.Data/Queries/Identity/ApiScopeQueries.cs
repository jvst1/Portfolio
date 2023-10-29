using IdentityServer4.EntityFramework.Entities;

namespace Portfolio.Data.Queries.Identity
{
    public static class ApiScopeQueries
    {
        public class GetAll : QueryBase<ApiResourceScope>
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
                var sql = @"SELECT APS.* FROM dbo.ApiScopes AS APS (NOLOCK) 			 
					JOIN dbo.ApiResourceScopes as APRS (NOLOCK) ON APRS.Scope = APS.Name
					JOIN dbo.ApiResources as APRE (NOLOCK) ON APRS.ApiResourceId = APRE.Id ";

                if (!string.IsNullOrWhiteSpace(IdResource))
                    AddCondition($"APRS.ApiResourceId = {IdResource}");

                if (!string.IsNullOrWhiteSpace(Search))
                    AddCondition(GetSearch(Search, "APRE.Name", "APRE.DisplayName"));

                var rawSql = sql + GetWhere();

                return rawSql;
            }
        }
        public class GetAllByClientID : QueryBase<ApiResourceScope>
        {
            public GetAllByClientID(int idClient)
            {
                IdClient = idClient;
            }
            public int IdClient { get; }

            public override string GetSql()
            {
                var sql = @$"SELECT APS.* FROM dbo.ApiScopes AS APS (NOLOCK) 			 
					WHERE NOT EXISTS(SELECT TOP(1) 1 FROM dbo.ClientScopes as CS (NOLOCK) WHERE CS.Scope = APS.Name AND CS.ClientId =  {IdClient})";

                var rawSql = sql + GetWhere();

                return rawSql;
            }
        }
    }
}