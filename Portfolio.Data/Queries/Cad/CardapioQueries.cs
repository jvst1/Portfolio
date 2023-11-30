using Portfolio.Domain.Entities.Cad;

namespace Portfolio.Data.Queries.Cad
{
    public class CardapioQueries
    {
        public class GetAll : QueryBase<Cardapio>
        {
            public string Search { get; }
            public Guid CodigoComerciante { get; }

            public GetAll(string search, Guid codigoComerciante)
            {
                Search = search;
                CodigoComerciante = codigoComerciante;
            }

            public override string GetSql()
            {
                var sql = @"SELECT * FROM cad.Cardapio (NOLOCK)";

                if (!string.IsNullOrEmpty(Search))
                    AddCondition(GetSearch(Search, "Nome", "Descricao"));

                AddCondition($"CodigoUsuario = @{nameof(CodigoComerciante)}");

                return sql + GetWhere();
            }
        }

        public class GetAllItensFromComerciante : QueryBase<Cardapio>
        {
            public Guid CodigoComerciante { get; }
            public bool VendaAtiva { get; }

            public GetAllItensFromComerciante(Guid codigoComerciante)
            {
                CodigoComerciante = codigoComerciante;
                VendaAtiva = true;
            }

            public override string GetSql()
            {
                var sql = @"SELECT * FROM cad.Cardapio (NOLOCK)";

                AddCondition($"CodigoUsuario = @{nameof(CodigoComerciante)}");
                AddCondition($"VendaAtiva = @{nameof(VendaAtiva)}");

                return sql + GetWhere();
            }
        }

        public class GetItemByIdFromComerciante : QueryBase<Cardapio>
        {
            public Guid Id { get; }
            public Guid CodigoComerciante { get; }

            public GetItemByIdFromComerciante(Guid id, Guid codigoComerciante)
            {
                Id = id;
                CodigoComerciante = codigoComerciante;
            }

            public override string GetSql()
            {
                var sql = @"SELECT * FROM cad.Cardapio (NOLOCK)";

                AddCondition($"Codigo = @{nameof(Id)}");
                AddCondition($"CodigoUsuario = @{nameof(CodigoComerciante)}");

                return sql + GetWhere();
            }
        }

        public class GetOfertas : QueryBase<Cardapio>
        {
            public GetOfertas()
            {
            }

            public override string GetSql()
            {
                var sql = @"SELECT * FROM cad.Cardapio (NOLOCK)";

                AddCondition("AplicarDesconto = 1");
                AddOrderBy("NEWID()");

                return sql + GetWhere() + GetOrderBy();
            }
        }
    }
}
