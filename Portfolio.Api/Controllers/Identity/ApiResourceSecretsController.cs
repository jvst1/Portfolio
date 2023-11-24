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
    public class ApiResourceSecretsController : PortfolioControllerBase
    {
        private readonly ApiResourceSecretsApplicationService _service;

        public ApiResourceSecretsController(ApiResourceSecretsApplicationService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = nameof(TipoPerfilUsuario.Administrador))]
        public ActionResult<IEnumerable<ApiResourceSecret>> Get(Request request) =>
           _service.GetAll(request, true);

        [HttpPost]
        public ActionResult<ApiResourceSecret> Post(ApiResourceSecretsRequest request)
        {
            var response = _service.Post(request);
            return CreatedAtAction("Get", new { id = response.Id }, response);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, ApiResourceSecretsRequest request)
        {
            if (id != request.Id) return BadRequest();
            _service.Put(id, request);
            return NoContent();
        }
    }
}