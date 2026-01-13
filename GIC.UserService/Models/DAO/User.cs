using System.ComponentModel.DataAnnotations;

namespace GIC.UserService.Models.DAO
{
    public class User : BaseAuditModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, MinLength(2)]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
