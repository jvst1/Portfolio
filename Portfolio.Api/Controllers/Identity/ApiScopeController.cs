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
    public class ApiScopeController : PortfolioControllerBase
    {
        private readonly ApiScopeApplicationService _service;

        public ApiScopeController(ApiScopeApplicationService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = nameof(TipoPerfilUsuario.Administrador))]
        public ActionResult<IEnumerable<ApiScope>> Get(Request request) =>
            _service.GetAll(request, true);

        [HttpGet("GetAllByClientID/{id}")]
        [Authorize(Roles = nameof(TipoPerfilUsuario.Administrador))]
        public ActionResult<IEnumerable<ApiScope>> GetAllByClientID(int id) =>
            _service.GetAllByClientID(id, true);

        [HttpPost]
        public ActionResult<ApiScope> Post(ApiScopeRequest request)
        {
            var response = _service.Post(request);
            return CreatedAtAction("Get", new { id = response.Id }, response);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, ApiScopeRequest request)
        {
            if (id != request.Id) return BadRequest();
            _service.Put(id, request);
            return NoContent();
        }
    }
}