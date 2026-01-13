using GIC.OrderService.Models.DTO;

namespace GIC.OrderService.Data.Services.Abstract
{
    public interface IOrderDataService
    {
        Task<List<OrderDto>> GetAllOrder();
        Task<List<OrderDto>> GetAllOrderByUserId();        
        Task<OrderDto> AddOrder(OrderDto orderDto);
        Task<OrderDto> GetAllOrderById(Guid orderId);
    }
}
