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
    public class ApiResourcesController : PortfolioControllerBase
    {
        private readonly ApiResourcesApplicationService _service;

        private readonly IMappingService<ApiResource> _mapper;

        public ApiResourcesController(IMappingService<ApiResource> mapper, ApiResourcesApplicationService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = nameof(TipoPerfilUsuario.Administrador))]
        public ActionResult<IEnumerable<ApiResourceResponse>> Get(Request request) =>
            _mapper.ConvertFromDomain<ApiResourceResponse>(_service.GetAll(request, true));

        [HttpPost]
        public ActionResult<ApiResourceResponse> Post(ApiResourceRequest request)
        {
            var response = _service.Post(request);
            return CreatedAtAction("Get", new { id = response.Id }, response);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, ApiResourceRequest request)
        {
            if (id != request.Id) return BadRequest();
            _service.Put(id, request);
            return NoContent();
        }
    }
}