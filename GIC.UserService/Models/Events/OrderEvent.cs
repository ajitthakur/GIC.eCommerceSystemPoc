using System.ComponentModel.DataAnnotations;

namespace GIC.UserService.Models.Events
{
    public class OrderCreatedEvent
    {
        public Guid Id { get; set; }        
        public Guid UserId { get; set; }        
        public string Product { get; set; }        
        public int Quantity { get; set; }        
        public double Price { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
