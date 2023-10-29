using Portfolio.Infrastructure.Enums;

namespace Portfolio.Infrastructure.Attributes
{
    public class IdentificadorDbService : Attribute
    {
        public string Identificador { get; }

        public OrmEngine OrmEngine { get; }

        public IdentificadorDbService(string identificador, OrmEngine ormEngine)
        {
            Identificador = identificador;
            OrmEngine = ormEngine;
        }
    }
}
