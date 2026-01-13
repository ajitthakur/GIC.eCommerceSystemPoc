using System.ComponentModel.DataAnnotations;

namespace GIC.UserService.Models.DTO
{
    public class UserDto : BaseAuditModel
    {
        public Guid Id { get; set; }

        [Required, MinLength(2)]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }
    }

    public class UserRequestDto 
    {
      
        [Required, MinLength(2)]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
