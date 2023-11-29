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
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;

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
            foreach (var entry in ChangeTracker.Entries().Where(IsEntryAddedOrModified))
                ProcessEntryAnnotations(entry);
        }

        private bool IsEntryAddedOrModified(EntityEntry entry)
        {
            return entry.State == EntityState.Added || entry.State == EntityState.Modified;
        }

        private void ProcessEntryAnnotations(EntityEntry entry)
        {
            var errors = new StringBuilder();
            var properties = entry.Entity.GetType().GetProperties();

            foreach (var property in properties)
                ApplyAnnotationsToProperty(entry, property, errors);

            if (errors.Length > 0)
                throw new PortfolioException($"As seguintes propriedades excederam o tamanho máximo permitido: {Environment.NewLine}{errors}");
        }

        private void ApplyAnnotationsToProperty(EntityEntry entry, PropertyInfo property, StringBuilder errors)
        {
            if (!(property.GetValue(entry.Entity) is string propertyValue)) 
                return;

            foreach (var attribute in property.GetCustomAttributes())
                ProcessAttribute(entry, property, attribute, propertyValue, errors);
        }

        private void ProcessAttribute(EntityEntry entry, PropertyInfo property, object attribute, string propertyValue, StringBuilder errors)
        {
            switch (attribute)
            {
                case ForceCase forceAttribute:
                    property.SetValue(entry.Entity, forceAttribute.Apply(propertyValue));
                    break;
                case Truncate truncateAttribute:
                    property.SetValue(entry.Entity, truncateAttribute.Apply(propertyValue));
                    break;
                case MaxLengthAttribute maxLengthAttribute:
                    ValidateLength(maxLengthAttribute.Length, propertyValue, property.Name, errors);
                    break;
            }
        }

        private void ValidateLength(int maxLength, string value, string propertyName, StringBuilder errors)
        {
            if (value.Length > maxLength)
                errors.AppendLine($"{propertyName} - Máximo: {maxLength} / Informado: {value.Length}");
        }


        protected override void OnModelCreating(ModelBuilder builder)
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

                builder.ApplyConfiguration(mappingClass);
            }

            // Registrar os raw query //migration: comentar para aplicar migration
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(IRawQueryResponse).IsAssignableFrom(p) && !p.IsInterface);

            foreach (var type in types)
                builder.Entity(type).HasNoKey();
        }
    }
}