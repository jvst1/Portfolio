using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portfolio.Domain.Base;

namespace Portfolio.Data.DbMappings.Base
{
    public static class DefaultMappings
    {
        public static Action<EntityTypeBuilder<T>> Ambos<T, TId>(string schema) where T : EntityBase<TId>
        {
            return c =>
            {
                c.ToTable(typeof(T).Name, schema);
                c.HasKey(x => x.Codigo);
                c.Property(x => x.ID).ValueGeneratedOnAddOrUpdate();
            };
        }
    }
}
