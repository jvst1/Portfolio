using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portfolio.Data.DbMappings.Base;
using Portfolio.Domain.Base.Interfaces.Data;
using Portfolio.Domain.Entities.Cad;
using Portfolio.Infrastructure.Enums;

namespace Portfolio.Data.DbMappings
{
    public interface IEfDbMapping<TEntity> : IDbMapping<TEntity>, IEntityTypeConfiguration<TEntity> where TEntity : class
    {
    }

    public class UsuarioMapping : DbMappingBase<Usuario>, IEfDbMapping<Usuario>
    {
        public override string DbService => Identificadores.Portfolio;
        public override string Schema => "cad";

        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            DefaultMappings.Ambos<Usuario, long>(Schema)(builder);
        }
    }
}
