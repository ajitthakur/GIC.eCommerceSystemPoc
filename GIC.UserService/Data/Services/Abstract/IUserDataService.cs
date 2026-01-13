using GIC.UserService.Models.DTO;

namespace GIC.UserService.Data.Services.Abstract
{
    public interface IUserDataService
    {
        Task<List<UserDto>> GetAllUser();
        Task<UserDto> GetUserByEmail(string email);
        Task<UserDto> AddUser(UserDto userDto);
        Task<UserDto> GetUserById(Guid userId);
    }
}
