using Portfolio.Domain.Base.Interfaces.Data;
using Portfolio.Domain.Entities.Cad;
using Portfolio.Domain.Messaging.Cad;
using Portfolio.Domain.Services.Cad;
using Portfolio.Infrastructure.Helpers;

namespace Portfolio.Application.Services.Cad
{
    public class CategoriaApplicationService
    {
        private readonly CategoriaDomainService _categoriaDomainService;
        private readonly IMappingService<Categoria> _mapper;

        public CategoriaApplicationService(CategoriaDomainService categoriaDomainService,
                                         IMappingService<Categoria> mapper)
        {
            _categoriaDomainService = categoriaDomainService;
            _mapper = mapper;
        }

        public List<CategoriaResponse> GetAll(Request request)
        {
            var search = request["search"]?.ToUpper();

            var categorias = _categoriaDomainService.GetAll(search);

            return _mapper.ConvertFromDomain<CategoriaResponse>(categorias);
        }

        public CategoriaResponse GetById(Guid id)
        {
            var entity = _categoriaDomainService.GetById(id);
            return _mapper.ConvertFromDomain<CategoriaResponse>(entity);
        }

        public CategoriaResponse Put(Guid id, CategoriaPutRequest request)
        {
            var entity = _mapper.ConvertToDomain(request);
            _categoriaDomainService.Update(id, entity);
            return _mapper.ConvertFromDomain<CategoriaResponse>(entity);
        }

        public CategoriaResponse Post(CategoriaPostRequest request)
        {
            var entity = _mapper.ConvertToDomain(request);
            _categoriaDomainService.Insert(entity);
            return _mapper.ConvertFromDomain<CategoriaResponse>(entity);
        }

        public void Delete(Guid id)
        {
            _categoriaDomainService.Remove(id);
        }
    }
}
