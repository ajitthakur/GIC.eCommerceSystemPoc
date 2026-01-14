using GIC.UserService.Data.Services.Abstract;
using GIC.UserService.Infrastructure.Abstract;
using GIC.UserService.Models.DTO;
using GIC.UserService.Models.Events;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace GIC.UserService.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserDataService _userDataService;
        private readonly IKafkaEventPublishService _kafkaEventPublishService;
        private readonly ConcurrentDictionary<Guid, List<string>> _orderDataCache;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserDataService userDataService,
            IKafkaEventPublishService kafkaEventPublishService,
            ConcurrentDictionary<Guid, List<string>> orderDataCache,
            ILogger<UserController> logger)
        {
            _userDataService = userDataService;
            _kafkaEventPublishService = kafkaEventPublishService;
            _logger = logger;
            _orderDataCache = orderDataCache;
        }

        /// <summary>
        /// This method is create User 
        /// </summary>
        /// <param name="userDto">User Dto</param>
        /// <returns>Created User Data</returns>
        [HttpPost]
        public async Task<IActionResult> Create(UserRequestDto userDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return ValidationProblem(ModelState);

                var userExist = _userDataService.GetUserByEmail(userDto.Email);

                if (userExist == null)
                {
                    return BadRequest("Invalid Request. Record Already exists");
                }

                var createUserData = await _userDataService.AddUser(new UserDto()
                {
                    Email = userDto.Email,
                    Name = userDto.Name,
                });

                var publishRequestMsg = new UserCreatedEvent
                {
                    Id = createUserData.Id,
                    Name = createUserData.Name,
                    Email = createUserData.Email,
                    CreatedOn = createUserData.CreatedOn,
                };

                await _kafkaEventPublishService.Publish<UserCreatedEvent>(createUserData.Id.ToString(), publishRequestMsg);

                return Created("User Created", createUserData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error : Create");
                return StatusCode(500);
            }
        }


        /// <summary>
        /// Get User Detail by User Id
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns>User Detail</returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            try
            {               
                var userExist = _userDataService.GetUserById(id);

                if (userExist == null)
                {
                    return NotFound();
                }

                _logger.LogInformation(JsonConvert.SerializeObject(_orderDataCache));                
                return Ok(new { UserDetail = userExist , OrderPlaced = _orderDataCache.ContainsKey(id)? JsonConvert.SerializeObject(_orderDataCache[id]) : ""});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error : Get");
                return StatusCode(500);
            }
        }
    }
}
