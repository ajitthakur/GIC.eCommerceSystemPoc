using GIC.OrderService.Data.Context;
using GIC.OrderService.Data.Services.Abstract;
using GIC.OrderService.Data.Context;
using GIC.OrderService.Data.Services.Abstract;
using GIC.OrderService.Models.DAO;
using GIC.OrderService.Models.DTO;
using Microsoft.EntityFrameworkCore;
using System;

namespace GIC.UserService.Data.Services
{
    public class OrderDataService : IOrderDataService
    {
        private readonly OrderDbContext _orderDbContext;

        public OrderDataService(OrderDbContext orderDbContext) 
        {
            _orderDbContext = orderDbContext;
        }

        public async Task<OrderDto> AddOrder(OrderDto orderDto)
        {
            var order = new Order()
            {
                CreatedOn = DateTime.UtcNow,                
                Price = orderDto.Price,
                Product = orderDto.Product,
                Quantity = orderDto.Quantity,
                UserId = orderDto.UserId
            };

            var result = await _orderDbContext.Orders.AddAsync(order);
            await _orderDbContext.SaveChangesAsync();
            orderDto.Id = order.Id;
            return orderDto;
        }

        public async Task<List<OrderDto>> GetAllOrder()
        {
            return await _orderDbContext.Orders.Select(x=> new OrderDto()
            {
                CreatedOn= x.CreatedOn,
                Id= x.Id,
                Price = x.Price,
                Product= x.Product, 
                Quantity= x.Quantity,   
                UserId = x.UserId   
            }).ToListAsync();
        }

        public async Task<OrderDto> GetAllOrderById(Guid orderId)
        {
            return await _orderDbContext.Orders.Where(x=>x.Id.Equals(orderId)).Select(x => new OrderDto()
            {
                CreatedOn = x.CreatedOn,
                Id = x.Id,
                Price = x.Price,
                Product = x.Product,
                Quantity = x.Quantity,
                UserId = x.UserId
            }).FirstOrDefaultAsync();
        }

        public Task<List<OrderDto>> GetAllOrderByUserId()
        {
            throw new NotImplementedException();
        }
    }
}
