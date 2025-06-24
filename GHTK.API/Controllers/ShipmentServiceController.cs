using GHTK.API.Models;
using GHTK.Repository;
using GHTK.Repository.Abstractions.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace GHTK.API.Controllers
{
    [ApiController]
    [Route("/services/shipment")]
    public class ShipmentServiceController : ControllerBase
    {
        private readonly ILogger<ShipmentServiceController> _logger;
        private readonly IOrderRepository _orderRepository;

        public ShipmentServiceController(ILogger<ShipmentServiceController>logger,IOrderRepository orderRepository)
        {
            _logger = logger;
            _orderRepository = orderRepository;
        }
        [HttpPost]
        [Route("order")]
        [Authorize]
        public async Task<IActionResult> SubmitOrder([FromBody] SubmitOrderRequest order)
        {
            _logger.LogInformation("Received order submission request: {@Order}", order);

            var partnerId = User.FindFirst("PartnerId")?.Value;

            if (string.IsNullOrEmpty(partnerId))
            {
                _logger.LogWarning("Partner ID not found in user claims.");
                return BadRequest(new ApiResult { Success = false, Message = "Partner ID is required." });
            }

            var orderEntity = new Order
            {
                PartnerId = partnerId,
                PickName = order.Order.PickName,
                PickAddress = order.Order.PickAddress,
                PickProvince = order.Order.PickProvince,
                PickDistrict = order.Order.PickDistrict,
                PickWard = order.Order.PickWard,
                PickTel = order.Order.PickTel,
                Tel = order.Order.Tel,
                Name = order.Order.Name,
                Address = order.Order.Address,
                Province = order.Order.Province,
                District = order.Order.District,
                Ward = order.Order.Ward,
                Hamlet = order.Order.Hamlet,
                IsFreeship = order.Order.IsFreeship,
                PickDate = DateTimeOffset.Now,
                PickMoney = order.Order.PickMoney,
                Note = order.Order.Note ?? string.Empty,
                Value = order.Order.Value,
                Transport = order.Order.Transport ?? "default",
                PickOption = order.Order.PickOption ?? "default",
                DeliverOption = order.Order.DeliverOption ?? "default",
                TrackingId=Guid.NewGuid().ToString(),
                Products = order.Products?.Select(p => new Product
                {
                    Name = p.Name,
                    Quantity = p.Quantity,
                    Weight = p.Weight,
                }).ToList() ?? new List<Product>()

            };
            

            await _orderRepository.CreateOrderAsync(orderEntity);

            var response = new SubmitOrderResponse
            {
                Success = true,
                Order = new SubmitOrderResponseOrder
                {
                    PartnerId = partnerId,
                    Label = "Order Label",
                    Area = 1,
                    Fee = 10000,
                    InsuranceFee = 5000,
                    TrackingId = orderEntity.TrackingId,
                    EstimatedPickTime = DateTime.UtcNow.AddHours(1).ToString("o"),
                    EstimatedDeliverTime = DateTime.UtcNow.AddDays(3).ToString("o"),
                    Products = orderEntity.Products.Select(p => new OrderProduct
                    {
                        Name = p.Name,
                        Quantity = p.Quantity,
                        Weight = p.Weight
                    }).ToList(),
                    StatusId = 1
                }
            };
            return Ok(response);
        }

        [HttpGet]

        [Route("v2/{id}")]
        [Authorize]
        public async Task<IActionResult> GetOderStatust(string id)
        {
            _logger.LogInformation("Received request for order status with ID: {Id}", id);

            var partnerId = User.FindFirst("PartnerId")?.Value;


            if (string.IsNullOrEmpty(partnerId))
            {
                _logger.LogWarning("Partner ID not found in user claims.");
                return BadRequest(new ApiResult { Success = false, Message = "Partner ID is required." });
            }

            var order = await _orderRepository.FindOrderAsync(id, partnerId);

            if (order == null)
            {
                return NotFound( new ApiResult { Success = false, Message = "Tracking Id not found." });
            }

            var result = new GetOrderStatusResponse
            {
                Success = true,
                Order = new StatusOrder
                {
                    LabelId = order.TrackingId,
                    PartnerId = partnerId,
                    Status = 1, // Assuming status is 1 for demonstration
                    Created = DateTimeOffset.UtcNow,
                    Modified = DateTimeOffset.UtcNow,
                    Message = "Order is currently in transit.",
                    PickDate = DateTimeOffset.UtcNow.AddHours(1),
                    DeliverDate = DateTimeOffset.UtcNow.AddDays(3),
                    CustomerFullname = order.Name,
                    CustomerTel = order.Tel,
                    Address = order.Address,
                    StorageDay = 0, // Assuming no storage day for this example
                    ShipMoney = 10000, // Example shipping cost
                    Insurance = 5000, // Example insurance cost
                }
                                
            };
            // Simulate fetching shipment details by ID
            return Ok(result);
        }

        [HttpPost]

        [Route("cancel/{id}")]
        [Authorize]
        public IActionResult CancelOrder(string id)
        {
            _logger.LogInformation("Received request to cancel order with ID: {Id}", id);
            var result = new ApiResult
            {
                Success = true,
                Message = "Order cancelled successfully"
            };
            // Simulate order cancellation
            return Ok();
        }

    }
}
