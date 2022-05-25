using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalanShoppingAccessLayer.Models;

namespace TalanShoppingAccessLayer.BusinessLogic.BLOrders
{
    public interface IOrdersBL
    {
        Task<List<Order>> GetAllOrders();
        Task<List<Order>> GetAllOrdersWithProducts();
        Task<Order> GetOrderById(int orderId);
        Task<Order> GetOrderByIdWithProducts(int orderId);
        Task AddOrder(Order order);
        Task UpdateOrder(Order order);
        Task DeleteOrderById(int orderId);
    }
}
