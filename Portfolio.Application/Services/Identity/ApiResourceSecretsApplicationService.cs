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
    public class ApiResourceSecretsApplicationService
    {
        private readonly ConfigurationDbContext _dbContext;
        private readonly IMappingService<ApiResourceSecret> _mapper;
        private readonly DbSet<ApiResourceSecret> _db;

        public ApiResourceSecretsApplicationService(IMappingService<ApiResourceSecret> mapper, ConfigurationDbContext context)
        {
            _dbContext = context;
            _mapper = mapper;
            _db = context.Set<ApiResourceSecret>();
        }

        public bool ApiResourceExists(int id) => _dbContext.ApiResources.Any(b => b.Id == id);

        public List<ApiResourceSecret> GetAll(Request request, bool nolock = false)
        {
            var idResource = request["idResource"];
            var search = request["search"].ToUpper();

            return _db.FromSqlRaw(new ApiResourceSecretsQueries.GetAll(search, idResource).GetSql()).ToList();
        }

        public ApiResourceSecret Post(ApiResourceSecretsRequest request)
        {
            var sender = _mapper.ConvertToDomain(request);
            request.Created = DateTime.Now;
            _db.Add(sender);
            _dbContext.SaveChanges();

            return sender;
        }

        public void Put(int id, ApiResourceSecretsRequest request)
        {
            var fromBd = _db.SingleOrDefault(s => s.Id == id);

            if (fromBd == null)
                throw new Exception("Não encontrado");

            fromBd.UpdateAllProperties(_mapper.ConvertToDomain(request));

            _dbContext.Update(fromBd);
            _dbContext.SaveChanges();
        }
    }
}
