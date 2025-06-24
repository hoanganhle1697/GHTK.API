using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClientAuthentication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientSourceController : ControllerBase
    {
        private readonly IClientSourceAuthenticationHandler _clientSourceAuthenticationHandler;
        private readonly ILogger<ClientSourceController> _logger;

        public ClientSourceController(IClientSourceAuthenticationHandler clientSourceAuthenticationHandler, ILogger<ClientSourceController> logger)
        {
            _clientSourceAuthenticationHandler = clientSourceAuthenticationHandler;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public IActionResult GetClientSource(string id)
        {
            _logger.LogInformation("Authenticating client with ID: {Id}", id);
            // Simulate fetching client source by ID
            if (_clientSourceAuthenticationHandler.Validate(id))
            {
                _logger.LogInformation("Client with ID: {Id} authenticated successfully", id);

                return Ok();
            }
            return Unauthorized();
        }

    }
}
