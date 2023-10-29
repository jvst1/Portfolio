using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Portfolio.Domain.Base.Interfaces.Data;
using Portfolio.Data.DbMappings;
using Portfolio.Infrastructure.Helpers;

namespace Portfolio.Data.Context
{
    public class EfContext : DbContext
    {
        private static readonly Dictionary<Guid, IDbMapping> LoadedMappings = new Dictionary<Guid, IDbMapping>();

        public string Identificador { get; }
        public CodigoUsuarioHelper CodigoUsuarioHelper { get; }

        public EfContext(DbContextOptions<EfContext> options, CodigoUsuarioHelper codigoHelper, string identificador) : base(options)
        {
            CodigoUsuarioHelper = codigoHelper;
            Identificador = identificador;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Fazendo o mapeamento com o banco de dados
            //Pega todas as classes que estão implementando a interface IMapping
            //Assim o Entity Framework é capaz de carregar os mapeamentos
            var dbMappingType = typeof(IEfDbMapping<>);

            var typesToMapping = (from x in Assembly.GetExecutingAssembly().GetTypes()
                                  from z in x.GetInterfaces()
                                  let y = x.BaseType
                                  where
                                      ((y != null && y.IsGenericType &&
                                       dbMappingType.IsAssignableFrom(y.GetGenericTypeDefinition())) ||
                                      (z.IsGenericType &&
                                       dbMappingType.IsAssignableFrom(z.GetGenericTypeDefinition()))) &&
                                      !x.IsAbstract
                                  select x).ToList();

            // Varrendo todos os tipos que são mapeamento 
            // Com ajuda do Reflection criamos as instancias 
            // e adicionamos no Entity Framework
            foreach (var mappingType in typesToMapping)
            {
                dynamic mappingClass = Activator.CreateInstance(mappingType);

                // ReSharper disable once PossibleNullReferenceException, é sabido neste ponto que o tipo terá BaseType
                LoadedMappings[mappingType.BaseType.GetGenericArguments()[0].GUID] = mappingClass;

                modelBuilder.ApplyConfiguration(mappingClass);
            }

            // Registrar os raw query
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(IRawQueryResponse).IsAssignableFrom(p) && !p.IsInterface);

            foreach (var type in types)
                modelBuilder.Entity(type).HasNoKey();
        }
    }
}