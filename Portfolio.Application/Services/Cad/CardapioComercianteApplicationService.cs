using Portfolio.Domain.Base.Interfaces.Data;
using Portfolio.Domain.Entities.Cad;
using Portfolio.Domain.Messaging.Cad;
using Portfolio.Domain.Services.Cad;
using Portfolio.Infrastructure.Helpers;

namespace Portfolio.Application.Services.Cad
{
    public class CardapioComercianteApplicationService
    {
        private readonly CardapioComercianteDomainService _cardapioComercianteDomainService;
        private readonly IMappingService<CardapioComerciante> _mapper;

        public CardapioComercianteApplicationService(CardapioComercianteDomainService cardapioComercianteDomainService,
                                         IMappingService<CardapioComerciante> mapper)
        {
            _cardapioComercianteDomainService = cardapioComercianteDomainService;
            _mapper = mapper;
        }

        public List<CardapioComercianteResponse> GetAll(Request request)
        {
            var search = request["search"]?.ToUpper();

            var cardapioComerciantes = _cardapioComercianteDomainService.GetAll(search);

            return _mapper.ConvertFromDomain<CardapioComercianteResponse>(cardapioComerciantes);
        }

        public CardapioComercianteResponse GetById(Guid id)
        {
            var entity = _cardapioComercianteDomainService.GetById(id);
            return _mapper.ConvertFromDomain<CardapioComercianteResponse>(entity);
        }

        public void Put(Guid id, CardapioComerciantePutRequest request)
        {
            var entity = _mapper.ConvertToDomain(request);
            _cardapioComercianteDomainService.Update(id, entity);
        }

        public CardapioComerciante Post(CardapioComerciantePostRequest request)
        {
            var sender = _mapper.ConvertToDomain(request);
            _cardapioComercianteDomainService.Insert(sender);
            return sender;
        }

        public void Delete(Guid id)
        {
            _cardapioComercianteDomainService.Remove(id);
        }
    }
}
