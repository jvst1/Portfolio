using Portfolio.Domain.Entities.Cad;

namespace Portfolio.Data.Queries.Cad
{
    public class CategoriaQueries
    {
        public class GetAll : QueryBase<Categoria>
        {
            public string Search { get; }

            public GetAll(string search)
            {
                Search = search;
            }

            public override string GetSql()
            {
                var sql = @"SELECT * FROM cad.Categoria (NOLOCK)";

                if (!string.IsNullOrEmpty(Search))
                    AddCondition(GetSearch(Search, "Nome"));

                return sql + GetWhere();
            }
        }
    }
}
