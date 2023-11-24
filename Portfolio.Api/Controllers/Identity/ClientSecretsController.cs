using System.Collections.Generic;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Application.Services.Identity;
using Portfolio.Domain.Base.Interfaces.Data;
using Portfolio.Domain.Messaging.Identity;
using Portfolio.Infrastructure.Enums;
using Portfolio.Infrastructure.Helpers;

namespace Portfolio.Api.Controllers.Identity
{
    [Authorize(Roles = nameof(TipoPerfilUsuario.Administrador))]
    public class ClientSecretsController : PortfolioControllerBase
    {
        private readonly ClientSecretsApplicationService _service;
        private readonly IMappingService<ClientSecret> _mapper;

        public ClientSecretsController(IMappingService<ClientSecret> mapper, ClientSecretsApplicationService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = nameof(TipoPerfilUsuario.Administrador))]
        public ActionResult<IEnumerable<ClientSecretResponse>> Get(Request request)
        {
            if (string.IsNullOrEmpty(request["clientId"]))
                return BadRequest();

            return _mapper.ConvertFromDomain<ClientSecretResponse>(_service.GetAll(request, true));
        }

        [HttpPost]
        public ActionResult<ClientSecret> Post(ClientSecretRequest request)
        {
            var response = _service.Post(request);
            return CreatedAtAction("Get", new { id = response.Id }, response);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, ClientSecretRequest request)
        {
            if (id != request.id)
                return BadRequest();

            _service.Put(id, request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _service.Delete(id);
            return Ok();
        }
    }
}