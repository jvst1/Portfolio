using Portfolio.Domain.Entities.Core;

namespace Portfolio.Data.Queries.Core
{
    public class EnvioEmailQueries
    {
        public class GetAll : QueryBase<EnvioEmail>
        {
            public DateTime DtCorte { get; }
            public bool Enviado { get; }
            public int Limite { get; }

            public GetAll(DateTime dtCorte, bool enviado, int limite)
            {
                DtCorte = dtCorte;
                Enviado = enviado;
                Limite = limite;
            }

            public override string GetSql()
            {
                var sql = @$"SELECT TOP {Limite} *
                            FROM core.EnvioEmail (NOLOCK) ";

                AddCondition($"CONVERT(DATETIME, DtInclusao, 103) <= CONVERT(DATETIME, @{nameof(DtCorte)}, 103)");
                AddCondition($"Enviado = @{nameof(Enviado)}");

                var rawSql = sql + GetWhere();
                return rawSql;
            }
        }
    }
}
