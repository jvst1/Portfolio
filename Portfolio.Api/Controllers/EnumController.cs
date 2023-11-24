using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Infrastructure.Enums;
using Portfolio.Infrastructure.Extensions;
using Portfolio.Infrastructure.Helpers;

namespace Portfolio.Api.Controllers
{
    [AllowAnonymous]
    public class EnumController : PortfolioControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get(Request request)
        {
            var lista = new List<string>
            {
                // Passar o tipo do value do enum quando não for int
                EnumExtensions.EnumToJson<TipoPerfilUsuario>(),
            };
            return string.Join("###", lista).Replace("}###{", ",");
        }
    }
}