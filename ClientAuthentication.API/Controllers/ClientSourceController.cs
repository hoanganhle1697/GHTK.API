using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClientAuthentication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientSourceController : ControllerBase
    {
        private readonly IClientSourceAuthenticationHandler _clientSourceAuthenticationHandler;

        public ClientSourceController(IClientSourceAuthenticationHandler clientSourceAuthenticationHandler)
        {
            _clientSourceAuthenticationHandler = clientSourceAuthenticationHandler;
        }

        [HttpGet("{id}")]
        public IActionResult GetClientSource(string id)
        {
            // Simulate fetching client source by ID
            if(_clientSourceAuthenticationHandler.Validate(id))
                return Ok(new { ClientSource = id, Valid = true });
            return Unauthorized();
        }

    }
}
