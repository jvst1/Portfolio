using System.Transactions;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Portfolio.Data.Queries.Identity;
using Portfolio.Domain.Base.Interfaces.Data;
using Portfolio.Domain.Messaging.Identity;
using Portfolio.Infrastructure.Extensions;
using Portfolio.Infrastructure.Helpers;
using Portfolio.Infrastructure.Security;

namespace Portfolio.Application.Services.Identity
{
    public class ClientSecretsApplicationService
    {
        private readonly ConfigurationDbContext _dbContext;
        private readonly IMappingService<ClientSecret> _mapper;
        private readonly DbSet<ClientSecret> _db;

        public ClientSecretsApplicationService(ConfigurationDbContext context, IMappingService<ClientSecret> mapper)
        {
            _dbContext = context;
            _mapper = mapper;
            _db = context.Set<ClientSecret>();
        }

        public List<ClientSecret> GetAll(Request request, bool nolock = false)
        {
            var clientId = request["clientId"];
            var search = request["search"].ToUpper();

            return _db.FromSqlRaw(new ClientSecretsQueries.GetAll(search, clientId).GetSql()).ToList();
        }

        public ClientSecret Post(ClientSecretRequest request)
        {
            using var ts = new TransactionScope();

            var sender = new ClientSecret
            {
                ClientId = request.ClientId,
                Created = DateTime.Now,
                Type = "JWK",
                Expiration = request.Expiration,
                Description = request.Description,
                Value = GerarJwk(request.ChavePEM)
            };

            _db.Add(sender);
            _dbContext.SaveChanges();

            ts.Complete();

            sender.Client = null;

            return sender;
        }

        public void Put(int id, ClientSecretRequest request)
        {
            var fromBd = _db.SingleOrDefault(r => r.Id == id);

            if (fromBd == null)
                throw new Exception("Não encontrado");

            using var ts = new TransactionScope();

            var clientSecret = _mapper.ConvertToDomain(request);

            clientSecret.Client = fromBd.Client;
            fromBd.UpdateAllProperties(clientSecret);

            if (!string.IsNullOrWhiteSpace(request.ChavePEM))
                clientSecret.Value = GerarJwk(request.ChavePEM);

            _dbContext.Update(fromBd);
            _dbContext.SaveChanges();

            ts.Complete();
        }

        public void Delete(int id)
        {
            var fromDb = _db.SingleOrDefault(p => p.Id == id);

            if (fromDb == null)
                throw new Exception("Não encontrado");

            _db.Remove(fromDb);
            _dbContext.SaveChanges();
        }

        private static string GerarJwk(string chavePem)
        {
            try
            {
                var provider = RSAKeys.ImportPublicKey(chavePem);
                return JsonConvert.SerializeObject(
                    JsonWebKeyConverter.ConvertFromRSASecurityKey(
                        new RsaSecurityKey(provider.ExportParameters(false))));
            }
            catch
            {
                throw new Exception("Não foi possível converter a chave PEM");
            }
        }
    }
}
