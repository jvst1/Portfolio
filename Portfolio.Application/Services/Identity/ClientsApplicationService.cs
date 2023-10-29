using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.Models;
using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Services.Cad;
using Portfolio.Data.Queries.Identity;
using Portfolio.Domain.Base.Interfaces.Data;
using Portfolio.Domain.Messaging.Identity;
using Portfolio.Infrastructure.Extensions;
using Portfolio.Infrastructure.Helpers;
using Client = IdentityServer4.EntityFramework.Entities.Client;

namespace Portfolio.Application.Services.Identity
{
    public class ClientsApplicationService
    {
        private readonly ConfigurationDbContext _dbContext;

        private readonly IMappingService<Client> _mapper;

        private readonly UsuarioApplicationService _usuarioService;

        public ClientsApplicationService(IMappingService<Client> mapper, ConfigurationDbContext context, UsuarioApplicationService usuarioService)
        {
            _dbContext = context;
            _mapper = mapper;
            _usuarioService = usuarioService;
        }

        public List<ClientResponse> GetAll(Request request, bool nolock = false)
        {
            var search = request["search"].ToUpper();

            var clients = _dbContext.Clients.FromSqlRaw(new ClientsQueries.GetAll(search).GetSql()).ToList();

            var clientResponse = _mapper.ConvertFromDomain<ClientResponse>(clients);

            foreach (var client in clientResponse)
                client.CodigoUsuario = _usuarioService.GetFirstByClientId(client.ClientId)?.Codigo;

            return clientResponse;
        }

        public Client Post(ClientRequest request)
        {
            var client = _mapper.ConvertToDomain(request);

            client.Created = DateTime.Now;
            client.AccessTokenType = (int)AccessTokenType.Reference;
            client.Enabled = true;

            _dbContext.Clients.Add(client);
            _dbContext.SaveChanges();

            _usuarioService.AtualizarClientIdIdentity(request.CodigoUsuario, client.ClientId);

            client.AllowedGrantTypes = new List<ClientGrantType>
            {
                new ClientGrantType {ClientId = client.Id, GrantType = "client_credentials"}
            };

            _dbContext.Update(client);
            _dbContext.SaveChanges();

            client.AllowedGrantTypes = null;

            return client;
        }

        public void Put(int id, ClientRequest request)
        {
            var client = _mapper.ConvertToDomain(request);
            var fromBd = _dbContext.Clients.Include(s => s.AllowedScopes).SingleOrDefault(p => p.Id == id);

            client.AccessTokenType = (int)AccessTokenType.Reference;

            if (client.Enabled)
                client.AllowedScopes = fromBd?.AllowedScopes;

            if (fromBd == null)
                throw new Exception("Não Encontrado");

            fromBd.UpdateAllProperties(client);
            fromBd.Updated = DateTime.Now;

            _dbContext.Update(fromBd);
            _dbContext.SaveChanges();

            _usuarioService.AtualizarClientIdIdentity(request.CodigoUsuario, client.ClientId);
        }
    }
}
