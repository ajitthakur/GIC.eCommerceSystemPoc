namespace GIC.UserService.Models.Events
{
    public class UserCreatedEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
