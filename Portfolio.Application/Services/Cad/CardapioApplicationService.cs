using Portfolio.Domain.Base.Interfaces.Data;
using Portfolio.Domain.Entities.Cad;
using Portfolio.Domain.Messaging.Cad;
using Portfolio.Domain.Services.Cad;
using Portfolio.Infrastructure.Helpers;

namespace Portfolio.Application.Services.Cad
{
    public class CardapioApplicationService
    {
        private readonly CardapioDomainService _cardapioDomainService;
        private readonly IMappingService<Cardapio> _mapper;

        public CardapioApplicationService(CardapioDomainService cardapioDomainService,
                                         IMappingService<Cardapio> mapper)
        {
            _cardapioDomainService = cardapioDomainService;
            _mapper = mapper;
        }

        public List<CardapioResponse> GetAll(Request request, Guid codigoComerciante)
        {
            var search = request["search"]?.ToUpper();

            var cardapios = _cardapioDomainService.GetAll(search, codigoComerciante);

            return _mapper.ConvertFromDomain<CardapioResponse>(cardapios);
        }

        public List<CardapioResponse> GetAllItensFromComerciante(Guid codigoComerciante)
        {
            var entity = _cardapioDomainService.GetAllItensFromComerciante(codigoComerciante);
            return _mapper.ConvertFromDomain<CardapioResponse>(entity);
        }

        public CardapioResponse GetItemByIdFromComerciante(Guid id, Guid codigoComerciante)
        {
            var entity = _cardapioDomainService.GetItemByIdFromComerciante(id, codigoComerciante);
            return _mapper.ConvertFromDomain<CardapioResponse>(entity);
        }

        public void Put(Guid id, CardapioPutRequest request)
        {
            var entity = _mapper.ConvertToDomain(request);
            _cardapioDomainService.Update(id, entity);
        }

        public Cardapio Post(CardapioPostRequest request)
        {
            var sender = _mapper.ConvertToDomain(request);
            _cardapioDomainService.Insert(sender);
            return sender;
        }

        public void Delete(Guid id, Guid codigoComerciante)
        {
            var entity = GetItemByIdFromComerciante(id, codigoComerciante);
            if (entity == null)
                throw new ArgumentException($"Não foi possível encontrar uma entidade com o ID {id} para remoção. Por favor, verifique se o ID é válido e tente novamente.");

            _cardapioDomainService.Remove(id);
        }
    }
}
