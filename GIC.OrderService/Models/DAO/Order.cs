using System.ComponentModel.DataAnnotations;

namespace GIC.OrderService.Models.DAO
{
    public class Order : BaseAuditModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public string Product { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        public double Price { get; set; }
    }
}
