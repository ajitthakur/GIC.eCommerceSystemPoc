using GIC.UserService.Data.Context;
using GIC.UserService.Data.Services.Abstract;
using GIC.UserService.Models.DAO;
using GIC.UserService.Models.DTO;
using Microsoft.EntityFrameworkCore;
using System;

namespace GIC.UserService.Data.Services
{
    public class UserDataService : IUserDataService
    {
        private readonly UserDbContext _userDbContext;

        public UserDataService(UserDbContext userDbContext) 
        {
            _userDbContext = userDbContext;
        }

        public async Task<UserDto> AddUser(UserDto userDto)
        {
            var user = new User()
            {              
                CreatedOn = DateTime.UtcNow,
                Email = userDto.Email,
                Name = userDto.Name,
            };

            var result = await _userDbContext.Users.AddAsync(user);
            await _userDbContext.SaveChangesAsync();
            userDto.Id = user.Id;
            return userDto;
        }

        public async Task<List<UserDto>> GetAllUser()
        {
            return await _userDbContext.Users.AsNoTracking().Select(x=> new UserDto()
            {
                Id = x.Id,
                CreatedOn= x.CreatedOn,
                Email= x.Email,
                Name= x.Name
            }).ToListAsync();
        }

        public async Task<UserDto> GetUserById(Guid userId)
        {
            return await _userDbContext.Users.AsNoTracking().Where(x=>x.Id.Equals(userId)).Select(x => new UserDto()
            {
                Id = x.Id,
                CreatedOn = x.CreatedOn,
                Email = x.Email,
                Name = x.Name
            }).FirstOrDefaultAsync();
        }

        public async Task<UserDto> GetUserByEmail(string email)
        {
            var user  = await _userDbContext.Users.AsNoTracking().Where(x => x.Email.Equals(email)).FirstOrDefaultAsync();

            if (user == null) {
                return null;
            }

            return new UserDto()
            {
                Id = user.Id,
                Email = email,
                Name = user.Name,
            };
        }
    }
}
