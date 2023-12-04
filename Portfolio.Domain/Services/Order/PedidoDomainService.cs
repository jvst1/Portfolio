using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Portfolio.Domain.Base;
using Portfolio.Domain.Entities.Cad;
using Portfolio.Domain.Entities.Order;
using Portfolio.Domain.Messaging.Order;
using Portfolio.Domain.Repositories.Order;
using Portfolio.Domain.Services.Cad;
using Portfolio.Domain.Services.Core;
using Portfolio.Infrastructure;
using Portfolio.Infrastructure.Enums;
using Portfolio.Infrastructure.Exceptions;
using Portfolio.Infrastructure.Helpers;
using System.Text;
using System.Transactions;

namespace Portfolio.Domain.Services.Order
{
    public class PedidoDomainService : CrudDomainServiceBase<IPedidoRepository, Pedido>
    {
        private readonly AppSettings _appSettings;
        private readonly IPedidoProdutoRepository _pedidoProdutoRepository;
        private readonly UsuarioDomainService _usuarioDomainService;
        private readonly UsuarioEnderecoDomainService _usuarioEnderecoDomainService;
        private readonly CardapioDomainService _cardapioDomainService;
        private readonly EnvioEmailDomainService _envioEmailDomainService;
        private ILogger<PedidoDomainService> _logger;

        public PedidoDomainService(IOptions<AppSettings> appSettings,
                                    ILogger<PedidoDomainService> logger,
                                    IPedidoRepository crudRepository,
                                    IPedidoProdutoRepository pedidoProdutoRepository,
                                    UsuarioDomainService usuarioDomainService,
                                    UsuarioEnderecoDomainService usuarioEnderecoDomainService,
                                    CardapioDomainService cardapioDomainService,
                                    EnvioEmailDomainService envioEmailDomainService) : base(crudRepository)
        {
            _appSettings = appSettings.Value;
            _logger = logger;
            _pedidoProdutoRepository = pedidoProdutoRepository;
            _usuarioDomainService = usuarioDomainService;
            _usuarioEnderecoDomainService = usuarioEnderecoDomainService;
            _cardapioDomainService = cardapioDomainService;
            _envioEmailDomainService = envioEmailDomainService;
        }

        public Pedido NovoPedido(PedidoPostRequest request, Guid codigoComerciante)
        {
            if (request.CodigoComerciante != codigoComerciante)
                throw new PortfolioException("O codigo do comerciante informado não é o mesmo informado na requisição");

            var comerciante = _usuarioDomainService.GetById(codigoComerciante);
            if (comerciante.TipoPerfil != TipoPerfilUsuario.Comerciante)
                throw new PortfolioException("Codigo do comerciante informado não pertence a um comerciante");

            var cliente = _usuarioDomainService.GetById(request.CodigoCliente);
            if (cliente.TipoPerfil != TipoPerfilUsuario.Cliente)
                throw new PortfolioException("Codigo do cliente informado não pertence a um cliente");

            var clienteEndereco = _usuarioEnderecoDomainService.GetById(request.CodigoClienteEndereco);
            if (clienteEndereco == null)
                throw new PortfolioException("O endereço informado no pedido não foi encontrado");

            if (clienteEndereco.CodigoUsuario != request.CodigoCliente)
                throw new PortfolioException("O endereço informado no pedido não pertence ao usuario que está fazendo o pedido");

            var cardapioQuantidadeDict = new Dictionary<Cardapio, PedidoProdutoPostRequest>();
            foreach (var produto in request.Produtos)
            {
                var item = _cardapioDomainService.GetItemByIdFromComerciante(produto.CodigoProduto, codigoComerciante);
                if (item == null)
                    throw new PortfolioException($"Produto {produto.Nome} selecionado no carrinho não pertence ao comerciante {comerciante.Nome}");

                cardapioQuantidadeDict.Add(item, produto);
            }

            var pedidoEntity = new Pedido
            {
                Codigo = Guid.NewGuid(),
                CodigoCliente = cliente.Codigo,
                CodigoClienteEndereco = clienteEndereco.Codigo,
                CodigoComerciante = codigoComerciante,
                FormaPagamento = request.FormaPagamento,
            };

            StringBuilder sb = new StringBuilder();
            var pedidoProdutoEntities = new List<PedidoProduto>();
            pedidoEntity.Valor = 0;
            foreach (var item in cardapioQuantidadeDict)
            {
                var produto = item.Key;
                var pedidoProdutoRequest = item.Value;

                var pedidoProdutoEntity = new PedidoProduto
                {
                    Codigo = Guid.NewGuid(),
                    CodigoPedido = pedidoEntity.Codigo,
                    CodigoProduto = produto.Codigo,
                    Quantidade = pedidoProdutoRequest.Quantidade,
                    Comentario = pedidoProdutoRequest.Comentario
                };
                pedidoProdutoEntities.Add(pedidoProdutoEntity);

                if (produto.AplicarDesconto)
                    pedidoEntity.Valor += produto.ValorDescontoFixo * pedidoProdutoRequest.Quantidade;
                else
                    pedidoEntity.Valor += produto.ValorProduto * pedidoProdutoRequest.Quantidade;

                sb.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2:C}</td><td>{3}</td><td>{4}</td></tr>",
                        produto.Nome, pedidoProdutoRequest.Quantidade, Util.FormatarDecimal(produto.AplicarDesconto ? produto.ValorDescontoFixo * pedidoProdutoRequest.Quantidade : produto.ValorProduto * pedidoProdutoRequest.Quantidade),
                        pedidoProdutoRequest.Comentario, $"{clienteEndereco.Logradouro}, {clienteEndereco.NroLogradouro}, {clienteEndereco.Bairro}, {clienteEndereco.Cidade}, {clienteEndereco.UF}");
            }

            if (pedidoEntity.Valor < comerciante.ValorMinimoPedido)
                throw new PortfolioException($"O valor de pedido mínimo para este estabelecimento comercial é de {Util.FormatarDecimal(comerciante.ValorMinimoPedido)}");

            using var ts = new TransactionScope();

            CrudRepository.Insert(pedidoEntity);
            _pedidoProdutoRepository.BulkInsert(pedidoProdutoEntities);

            ts.Complete();


            var email = _envioEmailDomainService.RegistrarEmailNovoPedido(comerciante, sb.ToString(), pedidoEntity.ID, pedidoEntity.Valor);
            _envioEmailDomainService.SendGmailAsync(email).GetAwaiter().GetResult();
            return pedidoEntity;
        }
    }
}
