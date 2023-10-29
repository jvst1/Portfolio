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
    public class ApiResourcesApplicationService
    {
        private readonly ConfigurationDbContext _dbContext;

        private readonly IMappingService<ApiResource> _mapper;

        public ApiResourcesApplicationService(IMappingService<ApiResource> mapper, ConfigurationDbContext context)
        {
            _dbContext = context;
            _mapper = mapper;
        }

        public List<ApiResource> GetAll(Request request, bool nolock = false)
        {
            var search = request["search"].ToUpper();

            var apiResources = _dbContext.ApiResources.FromSqlRaw(new ApiResourcesQueries.GetAll(search).GetSql()).ToList();

            return apiResources;
        }

        public void Put(int id, ApiResourceRequest request)
        {
            var sender = _dbContext.ApiResources.SingleOrDefault(p => p.Id == id);

            if (sender == null)
                throw new Exception("Não encontrado");

            sender.UpdateAllProperties(_mapper.ConvertToDomain(request));
            sender.Updated = DateTime.Now;

            _dbContext.Update(sender);
            _dbContext.SaveChanges();
        }

        public ApiResource Post(ApiResourceRequest request)
        {
            var sender = _mapper.ConvertToDomain(request);
            sender.Created = DateTime.Now;
            _dbContext.ApiResources.Add(sender);
            _dbContext.SaveChanges();
            return sender;
        }

        public bool ApiResourceExists(int id) => _dbContext.ApiResources.Any(b => b.Id == id);
    }
}