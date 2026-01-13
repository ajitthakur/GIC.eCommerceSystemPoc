using GIC.OrderService.Data.Services.Abstract;
using GIC.OrderService.Infrastructure.Abstract;
using GIC.OrderService.Models.DTO;
using GIC.OrderService.Models.Events;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace GIC.OrderService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderDataService _orderDataService;
        private readonly IKafkaEventPublishService _kafkaEventPublishService;
        private readonly ILogger<OrderController> _logger;
        private readonly ConcurrentDictionary<Guid, (string, string)> _userDataCache;

        public OrderController(IOrderDataService orderDataService, 
            IKafkaEventPublishService kafkaEventPublishService,
            ILogger<OrderController> logger,
            ConcurrentDictionary<Guid, (string, string)> userDataCache)
        {
            _orderDataService = orderDataService;
            _kafkaEventPublishService = kafkaEventPublishService;
            _logger = logger;
            _userDataCache = userDataCache;

        }

        /// <summary>
        /// Create a order for user and publish event
        /// </summary>
        /// <param name="orderDto">OrderDto</param>
        /// <returns>Order</returns>
        [HttpPost]
        public async Task<IActionResult> Create(OrderDto orderDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return ValidationProblem(ModelState);

                if (!_userDataCache.TryGetValue(orderDto.UserId, out (string, string) userData))
                {
                    return BadRequest("User not found");
                }

                var createdOrder = await _orderDataService.AddOrder(orderDto);

                var publishRequestMsg = new OrderCreatedEvent
                {
                    CreatedOn = createdOrder.CreatedOn,
                    Id = createdOrder.Id,
                    Price = createdOrder.Price,
                    Product = createdOrder.Product,
                    Quantity = createdOrder.Quantity,
                    UserId  = createdOrder.UserId,                    
                };

                await _kafkaEventPublishService.Publish<OrderCreatedEvent>(publishRequestMsg.Id.ToString(), publishRequestMsg);

                return Created("Order Created", createdOrder);  
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error : Create");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Get Order by Id
        /// </summary>
        /// <param name="id">Order Id</param>
        /// <returns>Order Detail, User Detail</returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            try
            {
                var orderDetail = await _orderDataService.GetAllOrderById(id);
                _logger.LogInformation(JsonConvert.SerializeObject(_userDataCache));
                return Ok(new { orderDetail, User = (_userDataCache.ContainsKey(orderDetail.UserId) ? _userDataCache[orderDetail.UserId] : ("","") )});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error : GetOrderById");
                return StatusCode(500);
            }
        }


        [HttpGet]
        [Route("Users")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {                
                return Ok(_userDataCache);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error : GetUsers");
                return StatusCode(500);
            }
        }
    }
}
