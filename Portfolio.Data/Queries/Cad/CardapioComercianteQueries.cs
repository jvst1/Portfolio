using Portfolio.Domain.Entities.Cad;

namespace Portfolio.Data.Queries.Cad
{
    public class CardapioComercianteQueries
    {
        public class GetAll : QueryBase<CardapioComerciante>
        {
            public string Search { get; }

            public GetAll(string search)
            {
                Search = search;
            }

            public override string GetSql()
            {
                var sql = @"SELECT * FROM cad.CardapioComerciante (NOLOCK)";

                if (!string.IsNullOrEmpty(Search))
                    AddCondition(GetSearch(Search, "Nome", "Descricao"));

                return sql + GetWhere();
            }
        }
    }
}
