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
    public class ClientScopesController : PortfolioControllerBase
    {
        private readonly ClientScopesApplicationService _service;


        public ClientScopesController(ClientScopesApplicationService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = nameof(TipoPerfilUsuario.Administrador))]
        public ActionResult<IEnumerable<ClientScope>> Get(Request request)
        {
            if (string.IsNullOrWhiteSpace(request["clientId"]))
                return BadRequest();

            return _service.GetAll(request, true);
        }

        [HttpPost]
        public ActionResult<ClientScope> Post(ClientScopesRequest request)
        {
            _ = _service.Post(request);
            return CreatedAtAction("Get", request);
        }

        [HttpDelete("{id}")]
        public ActionResult<ClientScope> Delete(int id)
        {
            _service.Delete(id);
            return Ok();
        }
    }
}