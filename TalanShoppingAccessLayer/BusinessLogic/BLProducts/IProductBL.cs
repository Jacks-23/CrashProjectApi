using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalanShoppingAccessLayer.Models;

namespace TalanShoppingAccessLayer.BusinessLogic.BLProducts
{
    public interface IProductBL
    {
        Task<List<Product>> GetAllProducts();
        Task<Product> GetProductById(int productId);
        Task AddProduct(Product product);
        Task UpdateProduct(Product product);
        Task DeleteProductById(int productId);
    }
}
