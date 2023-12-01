use portfolio;
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE SCHEMA [cad]
GO

CREATE TABLE [cad].[Usuario](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Codigo] [uniqueidentifier] NOT NULL,
	[Identificador] [varchar](30) NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[Nome] [varchar](255) NOT NULL,
	[DocumentoFederal] [varchar](14) NOT NULL,
	[ImageUrl] [nvarchar](max) NULL,
	[TelefoneCelular] [varchar](100) NULL,
	[TempoEntrega] [varchar](100) NULL,
	[ValorMinimoPedido] [decimal](17, 2) NULL,
	[Tags] [varchar](100) NULL,
	[Situacao] [int] NULL,
	[TipoPerfil] [int] NULL,
	[DtLiberacao] [datetime] NULL,
	[DtBloqueio] [datetime] NULL,
	[SenhaAcesso] [varchar](100) NULL,
	[SenhaAPI] [varchar](100) NULL,
	[DtInclusao] [datetime] NULL,
	[DtAlteracao] [datetime] NULL,
	[RefreshToken] [varchar](100) NULL,
	[DtExpiracaoRefreshToken] [datetime] NULL,
	[TokenSenha] [varchar](500) NULL,
	[DtValidadeTokenSenha] [datetime] NULL,
	[ClientIdIdentityServer] [nchar](400) NULL,
 CONSTRAINT [PK_Usuario] PRIMARY KEY NONCLUSTERED 
(
	[Codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [cad].[UsuarioEndereco](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Codigo] [uniqueidentifier] NOT NULL,
	[CodigoUsuario] [uniqueidentifier] NOT NULL,
	[CEP] [varchar](8) NULL,
	[Logradouro] [varchar](100) NULL,
	[NroLogradouro] [varchar](15) NULL,
	[Bairro] [varchar](50) NULL,
	[Complemento] [varchar](50) NULL,
	[Cidade] [varchar](40) NULL,
	[UF] [varchar](2) NULL,
	[EnderecoDesde] [datetime] NULL,
	[EnderecoPrincipal] [bit] NULL,
	[DtInclusao] [datetime] NULL,
	[DtAlteracao] [datetime] NULL,
 CONSTRAINT [PK_UsuarioEndereco] PRIMARY KEY NONCLUSTERED 
(
	[Codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 85) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [cad].[UsuarioEndereco]  WITH CHECK ADD  CONSTRAINT [FK_UsuarioEndereco_Usuario] FOREIGN KEY([CodigoUsuario])
REFERENCES [cad].[Usuario] ([Codigo])

GO

ALTER TABLE [cad].[UsuarioEndereco] CHECK CONSTRAINT [FK_UsuarioEndereco_Usuario]
GO

CREATE TABLE [cad].[Categoria](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Codigo] [uniqueidentifier] NOT NULL,
	[ImageUrl] [nvarchar](max) NOT NULL,
	[Nome] [varchar](255) NOT NULL,
	[DtInclusao] [datetime] NULL,
	[DtAlteracao] [datetime] NULL,
 CONSTRAINT [PK_Categoria] PRIMARY KEY NONCLUSTERED 
(
	[Codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 85) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [cad].[Cardapio](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Codigo] [uniqueidentifier] NOT NULL,
	[CodigoUsuario] [uniqueidentifier] NOT NULL,
	[Nome] [varchar](255) NOT NULL,
	[Descricao] [varchar](255) NOT NULL,
	[ImageUrl] [nvarchar](max) NOT NULL,
	[ValorProduto] [decimal](17, 2) NULL,
	[ValorDescontoFixo] [decimal](17, 2) NULL,
	[ValorDescontoPercentual] [decimal](17, 2) NULL,
	[VendaAtiva] [bit] NULL,
	[AplicarDesconto] [bit] NULL,
	[DtInclusao] [datetime] NULL,
	[DtAlteracao] [datetime] NULL,
 CONSTRAINT [PK_Cardapio] PRIMARY KEY NONCLUSTERED 
(
	[Codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [cad].[Cardapio]  WITH CHECK ADD  CONSTRAINT [FK_Cardapio_Usuario] FOREIGN KEY([CodigoUsuario])
REFERENCES [cad].[Usuario] ([Codigo])
GO

CREATE TABLE [order].[Pedido](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Codigo] [uniqueidentifier] NOT NULL,
	[CodigoCliente] [uniqueidentifier] NOT NULL,
	[CodigoClienteEndereco] [uniqueidentifier] NOT NULL,
	[CodigoComerciante] [uniqueidentifier] NOT NULL,
	[Valor] [decimal](17, 2) NOT NULL,
	[FormaPagamento] [varchar](100) NOT NULL,
	[DtInclusao] [datetime] NULL,
 CONSTRAINT [PK_Pedido] PRIMARY KEY NONCLUSTERED 
(
	[Codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 85) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [order].[Pedido]  WITH CHECK ADD  CONSTRAINT [FK_Pedido_Usuario_CodigoCliente] FOREIGN KEY([CodigoCliente])
REFERENCES [cad].[Usuario] ([Codigo])
GO

ALTER TABLE [order].[Pedido] CHECK CONSTRAINT [FK_Pedido_Usuario_CodigoCliente]
GO

ALTER TABLE [order].[Pedido]  WITH CHECK ADD  CONSTRAINT [FK_Pedido_Usuario_CodigoComerciante] FOREIGN KEY([CodigoComerciante])
REFERENCES [cad].[Usuario] ([Codigo])
GO

ALTER TABLE [order].[Pedido] CHECK CONSTRAINT [FK_Pedido_Usuario_CodigoComerciante]
GO

ALTER TABLE [order].[Pedido]  WITH CHECK ADD  CONSTRAINT [FK_Pedido_UsuarioEndereco] FOREIGN KEY([CodigoClienteEndereco])
REFERENCES [cad].[UsuarioEndereco] ([Codigo])
GO

ALTER TABLE [order].[Pedido] CHECK CONSTRAINT [FK_Pedido_UsuarioEndereco]
GO

CREATE TABLE [order].[PedidoProduto](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Codigo] [uniqueidentifier] NOT NULL,
	[CodigoPedido] [uniqueidentifier] NOT NULL,
	[CodigoProduto] [uniqueidentifier] NOT NULL,
	[Quantidade] [int] NOT NULL,
	[Comentario] [varchar](max) NULL,
	[DtInclusao] [datetime] NULL,
 CONSTRAINT [PK_PedidoProduto] PRIMARY KEY NONCLUSTERED 
(
	[Codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 85) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [order].[PedidoProduto]  WITH CHECK ADD  CONSTRAINT [FK_PedidoProduto_Cardapio] FOREIGN KEY([CodigoProduto])
REFERENCES [cad].[Cardapio] ([Codigo])
GO

ALTER TABLE [order].[PedidoProduto] CHECK CONSTRAINT [FK_PedidoProduto_Cardapio]
GO

ALTER TABLE [order].[PedidoProduto]  WITH CHECK ADD  CONSTRAINT [FK_PedidoProduto_Pedido] FOREIGN KEY([CodigoPedido])
REFERENCES [order].[Pedido] ([Codigo])
GO

ALTER TABLE [order].[PedidoProduto] CHECK CONSTRAINT [FK_PedidoProduto_Pedido]
GO
