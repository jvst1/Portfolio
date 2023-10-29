using System.Transactions;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Portfolio.Data.Queries.Identity;
using Portfolio.Domain.Messaging.Identity;
using Portfolio.Infrastructure.Helpers;

namespace Portfolio.Application.Services.Identity
{
    public class ClientScopesApplicationService
    {
        private readonly ConfigurationDbContext _dbContext;
        private readonly DbSet<ClientScope> _db;

        public ClientScopesApplicationService(ConfigurationDbContext context)
        {
            _dbContext = context;
            _db = context.Set<ClientScope>();
        }

        public bool ClientsExists(int id) => _dbContext.Clients.Any(b => b.Id == id);

        public List<ClientScope> GetAll(Request request, bool nolock = false)
        {
            var clientId = request["clientId"];
            var search = request["search"].ToUpper();

            return _db.FromSqlRaw(new ClientScopesQueries.GetAll(search, clientId).GetSql(), clientId).ToList();
        }

        public ClientScope Post(ClientScopesRequest request)
        {
            var client = _dbContext.Clients.Include(p => p.AllowedScopes)
                .SingleOrDefault(p => p.Id == request.ClientId);

            if (client != null && !client.Enabled)
                throw new Exception("Cliente inativo, não é possivel adicionar ou atualizar os Scopes");

            var listaScopes = new List<string>();

            if (!string.IsNullOrEmpty(request.Scopes))
                listaScopes = JsonConvert.DeserializeObject<List<string>>(request.Scopes).ToList();

            using var ts = new TransactionScope();

            foreach (var scope in listaScopes.Where(scope => client != null && client.AllowedScopes.All(a => a.Scope != scope)))
                client?.AllowedScopes.Add(new ClientScope
                {
                    ClientId = request.ClientId,
                    Scope = scope
                });

            _dbContext.SaveChanges();

            ts.Complete();

            return client?.AllowedScopes.SingleOrDefault(cli => cli.Id == request.ClientId);
        }

        public void Delete(int id)
        {
            var sender = _dbContext.Clients.Include(r => r.AllowedScopes)
                .SingleOrDefault(p => p.AllowedScopes.Any(c => c.Id == id));
            if (sender == null)
                throw new Exception("Não encontrado");

            sender.AllowedScopes.RemoveAll(p => p.Id == id);

            _dbContext.Update(sender);
            _dbContext.SaveChanges();
        }
    }
}
