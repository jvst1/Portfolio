using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Portfolio.Domain.Base.Interfaces.Data;
using Portfolio.Data.DbMappings;
using Portfolio.Infrastructure.Helpers;
using Microsoft.Extensions.Options;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using AutoMapper.Configuration;
using Portfolio.Domain.Entities.Cad;
using Portfolio.Infrastructure.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Portfolio.Infrastructure.Exceptions;

namespace Portfolio.Data.Context
{
    public class EfContext : DbContext
    {
        private static readonly Dictionary<Guid, IDbMapping> LoadedMappings = new Dictionary<Guid, IDbMapping>();

        public string Identificador { get; }
        public CodigoUsuarioHelper CodigoUsuarioHelper { get; }

        //public EfContext(Microsoft.Extensions.Configuration.IConfiguration configuration) : base(new DbContextOptionsBuilder<EfContext>().UseSqlServer(configuration.GetConnectionString("Portfolio"), o => o.MigrationsAssembly("Portfolio.Api")).Options)
        //{
        //} //migration: Descomentar para aplicar migration

        public EfContext(DbContextOptions<EfContext> options, CodigoUsuarioHelper codigoHelper, string identificador) : base(options)
        {
            CodigoUsuarioHelper = codigoHelper;
            Identificador = identificador;
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ValidaDtInclusao();
            ApplyCustomAnnotation();

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        private void ValidaDtInclusao()
        {
            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("DtInclusao") != null))
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Property("DtInclusao").CurrentValue = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Property("DtInclusao").IsModified = false;
                        break;
                    case EntityState.Detached:
                    case EntityState.Unchanged:
                    case EntityState.Deleted:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void ApplyCustomAnnotation()
        {
            foreach (var entry in ChangeTracker.Entries()
                .Where(entry => entry.State == EntityState.Added || entry.State == EntityState.Modified))
            {
                var type = entry.Entity.GetType();

                var errosDeTamanho = new StringBuilder();

                foreach (var property in type.GetProperties())
                {
                    if (!(property.GetValue(entry.Entity) is string propVal)) continue;

                    foreach (var attr in property.GetCustomAttributes())
                    {
                        switch (attr)
                        {
                            case ForceCase forceAttribute:
                                property.SetValue(entry.Entity, forceAttribute.Apply(propVal));
                                break;
                            case Truncate truncateAttribute:
                                property.SetValue(entry.Entity, truncateAttribute.Apply(propVal));
                                break;
                            case MaxLengthAttribute maxLengthAttribute:
                                ValidaTamanho(maxLengthAttribute.Length, propVal, property.Name, errosDeTamanho);
                                break;
                        }
                    }
                }

                if (errosDeTamanho.Length > 0)
                    throw new PortfolioException($"As seguintes propriedades excederam o tamanho máximo permitido: {Environment.NewLine}{errosDeTamanho}");
            }
        }
        private static void ValidaTamanho(int maxLength, string propVal, string propertyName, StringBuilder strBuilder)
        {
            if (propVal.Length > maxLength)
                strBuilder.AppendLine($"{propertyName} - Máximo: {maxLength} / Informado: {propVal.Length}");
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

            // Registrar os raw query //migration: comentar para aplicar migration
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(IRawQueryResponse).IsAssignableFrom(p) && !p.IsInterface);

            foreach (var type in types)
                modelBuilder.Entity(type).HasNoKey();
        }
    }
}