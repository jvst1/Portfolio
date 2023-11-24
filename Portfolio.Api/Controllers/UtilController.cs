using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Controllers;
using Portfolio.Infrastructure;

namespace Portfolio.API.Controllers
{
    [Authorize]
    public class UtilController : PortfolioControllerBase
    {
        [HttpGet("ValidaDocumento/{documento}")]
        public ActionResult<bool> ValidaDocumento(string documento)
        {
            return Ok(Util.ValidaDocumento(documento));
        }

        [HttpGet("RemoveAcentos/{texto}")]
        public ActionResult<string> RemoveAcentos(string texto)
        {
            return Ok(Util.RemoveAcentos(texto));
        }
    }
}