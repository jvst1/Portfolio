using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Portfolio.Data.Queries.Identity;
using Portfolio.Domain.Base.Interfaces.Data;
using Portfolio.Domain.Messaging.Identity;
using Portfolio.Infrastructure.Extensions;
using Portfolio.Infrastructure.Helpers;

namespace Portfolio.Application.Services.Identity
{
    public class ApiScopeApplicationService
    {
        private readonly ConfigurationDbContext _dbContext;
        private readonly IMappingService<ApiScope> _mapper;
        private readonly DbSet<ApiScope> _db;

        public ApiScopeApplicationService(IMappingService<ApiScope> mapper, ConfigurationDbContext context)
        {
            _dbContext = context;
            _mapper = mapper;
            _db = context.Set<ApiScope>();
        }

        public bool ApiResourceExists(int id) => _dbContext.ApiResources.Any(b => b.Id == id);

        public List<ApiScope> GetAll(Request request, bool nolock = false)
        {
            var idResource = request["idResource"];
            var search = request["search"]?.ToUpper();

            return _db.FromSqlRaw(new ApiScopeQueries.GetAll(search, idResource).GetSql()).ToList();
        }

        public List<ApiScope> GetAllByClientID(int id, bool nolock = false)
        {
            return _db.FromSqlRaw(new ApiScopeQueries.GetAllByClientID(id).GetSql()).ToList();
        }

        public ApiScope Post(ApiScopeRequest request)
        {
            var sender = _mapper.ConvertToDomain(request);
            _db.Add(sender);
            ApiResourceScope apiResourceScope = new ApiResourceScope()
            {
                ApiResourceId = request.ApiResourceId,
                Scope = request.Name
            };
            var resource = _dbContext.ApiResources.Include(p => p.Scopes).SingleOrDefault(p => p.Id == request.ApiResourceId);
            resource.Scopes.Add(apiResourceScope);

            _dbContext.SaveChanges();

            return sender;
        }

        public void Put(int id, ApiScopeRequest request)
        {
            var fromBd = _db.SingleOrDefault(s => s.Id == id);

            if (fromBd == null)
                throw new Exception("Não encontrado");


            if (fromBd.Name != request.Name)
            {
                var resource = _dbContext.ApiResources.Include(p => p.Scopes).SingleOrDefault(p => p.Id == request.ApiResourceId);
                var apiResourceScope = resource.Scopes.SingleOrDefault(p => p.Scope == fromBd.Name);
                if (apiResourceScope != null)
                    apiResourceScope.Scope = request.Name;
            }

            fromBd.UpdateAllProperties(_mapper.ConvertToDomain(request));
            _dbContext.Update(fromBd);
            _dbContext.SaveChanges();
        }
    }
}
