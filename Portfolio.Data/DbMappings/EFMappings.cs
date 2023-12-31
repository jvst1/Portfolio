﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portfolio.Data.DbMappings.Base;
using Portfolio.Domain.Base.Interfaces.Data;
using Portfolio.Domain.Entities.Cad;
using Portfolio.Domain.Entities.Core;
using Portfolio.Domain.Entities.Order;
using Portfolio.Infrastructure.Enums;

namespace Portfolio.Data.DbMappings
{
    public interface IEfDbMapping<TEntity> : IDbMapping<TEntity>, IEntityTypeConfiguration<TEntity> where TEntity : class
    {
    }

    #region Schema Cad

    public class UsuarioMapping : DbMappingBase<Usuario>, IEfDbMapping<Usuario>
    {
        public override string DbService => Identificadores.Portfolio;
        public override string Schema => "cad";

        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            DefaultMappings.Ambos<Usuario, long>(Schema)(builder);
        }
    }
    public class UsuarioEnderecoMapping : DbMappingBase<UsuarioEndereco>, IEfDbMapping<UsuarioEndereco>
    {
        public override string DbService => Identificadores.Portfolio;
        public override string Schema => "cad";

        public void Configure(EntityTypeBuilder<UsuarioEndereco> builder)
        {
            DefaultMappings.Ambos<UsuarioEndereco, long>(Schema)(builder);
        }
    }

    public class CategoriaMapping : DbMappingBase<Categoria>, IEfDbMapping<Categoria>
    {
        public override string DbService => Identificadores.Portfolio;
        public override string Schema => "cad";

        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            DefaultMappings.Ambos<Categoria, long>(Schema)(builder);
        }
    }

    public class CardapioMapping : DbMappingBase<Cardapio>, IEfDbMapping<Cardapio>
    {
        public override string DbService => Identificadores.Portfolio;
        public override string Schema => "cad";

        public void Configure(EntityTypeBuilder<Cardapio> builder)
        {
            DefaultMappings.Ambos<Cardapio, long>(Schema)(builder);
        }
    }

    #endregion

    #region Schema core

    public class EnvioEmailMapping : DbMappingBase<EnvioEmail>, IEfDbMapping<EnvioEmail>
    {
        public override string DbService => Identificadores.Portfolio;
        public override string Schema => "core";

        public void Configure(EntityTypeBuilder<EnvioEmail> builder)
        {
            DefaultMappings.Ambos<EnvioEmail, int>(Schema)(builder);
        }
    }

    public class EnvioEmailAnexoMapping : DbMappingBase<EnvioEmailAnexo>, IEfDbMapping<EnvioEmailAnexo>
    {
        public override string DbService => Identificadores.Portfolio;
        public override string Schema => "core";

        public void Configure(EntityTypeBuilder<EnvioEmailAnexo> builder)
        {
            DefaultMappings.Ambos<EnvioEmailAnexo, int>(Schema)(builder);
        }
    }

    #endregion

    #region Schema order

    public class PedidoMapping : DbMappingBase<Pedido>, IEfDbMapping<Pedido>
    {
        public override string DbService => Identificadores.Portfolio;
        public override string Schema => "order";

        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            DefaultMappings.Ambos<Pedido, long>(Schema)(builder);
        }
    }

    public class PedidoProdutoMapping : DbMappingBase<PedidoProduto>, IEfDbMapping<PedidoProduto>
    {
        public override string DbService => Identificadores.Portfolio;
        public override string Schema => "order";

        public void Configure(EntityTypeBuilder<PedidoProduto> builder)
        {
            DefaultMappings.Ambos<PedidoProduto, long>(Schema)(builder);
        }
    }

    #endregion
}
