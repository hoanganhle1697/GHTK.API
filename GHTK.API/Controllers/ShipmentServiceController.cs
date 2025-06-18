using GHTK.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GHTK.API.Controllers
{
    [ApiController]
    [Route("/services/shipment")]
    public class ShipmentServiceController : ControllerBase
    {
        [HttpPost]
        [Authorize]
        [Route("order")]
        public IActionResult CreateOder([FromBody] CreateOder shipment)
        {
            return(Ok(new
            {
                message = "Order created successfully",
                shipmentId = Guid.NewGuid().ToString()
            }));
        }
    }
}
