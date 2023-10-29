using IdentityServer4.EntityFramework.Entities;

namespace Portfolio.Data.Queries.Identity
{
    public static class ClientsQueries
    {
        public class GetAll : QueryBase<Client>
        {
            public GetAll(string search)
            {
                Search = search;
            }

            public string Search { get; }

            public override string GetSql()
            {
                var sql = @"SELECT a.*, CodigoOperador = 1 FROM dbo.Clients as a (NOLOCK)";

                if (!string.IsNullOrWhiteSpace(Search))
                    AddCondition(GetSearch(Search, "ClientId", IsNull("ClientName"), IsNull("Description")));

                var rawSql = sql + GetWhere();
                return rawSql;
            }
        }
    }
}