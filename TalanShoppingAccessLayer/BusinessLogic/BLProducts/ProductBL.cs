using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalanShoppingAccessLayer.Models;

namespace TalanShoppingAccessLayer.BusinessLogic.BLProducts
{
    public class ProductBL : IProductBL
    {
        private IDbConnection db;

        public ProductBL(string connectionString)
        {
            db = new SqlConnection(connectionString);
        }

        public async Task<List<Product>> GetAllProducts()
        {
            var sql = "Select [ProductId], [Name], [Price], [Picture], [Description]" +
                " From Products"; 

            var queryResult = await db.QueryAsync<Product>(sql);
            List<Product> products = queryResult.ToList();

            return products;
        }
        public async Task<Product> GetProductById(int productId)
        {
            var sql = "Select [ProductId], [Name], [Price], [Picture], [Description]" +
                " From Products" +
                $" Where ProductId = {productId}";

            var queryResult = await db.QueryAsync<Product>(sql, productId);

            if (!queryResult.Any())
                throw new Exception("No product found");
            
            Product product = queryResult.First();
            
            return product;

        }
        public async Task AddProduct(Product product)
        {
            try
            {
                if (product.Name == "")
                    throw new Exception("A product needs a name");
                var sql = "Insert Into Products (Name, Price, Picture, Description)" +
                " Values(@Name, @Price, @Picture, @Description)" +
                " Select Cast(Scope_Identity() as int)";

                await db.ExecuteAsync(sql, product);

            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
            
        }
        public async Task UpdateProduct(Product product)
        {
            try
            {
                if (product.Name == "")
                    throw new Exception("A product needs a name");

                var sql = "Update Products Set" +
                " Name = @Name," +
                " Price = @Price," +
                " Picture = @Picture," +
                " Description = @Description" +
                " Where ProductId = @ProductId";

                bool productExists = await ValidateProductExists(product.ProductId);

                if (productExists)
                    await db.ExecuteAsync(sql, product);
                else
                    throw new Exception("The product doesn't exist");

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }     
        }
        private async Task<bool> ValidateProductExists(int productId)
        {
            var sql = "Select [ProductId]" +
                " From Products" +
                $" Where ProductId = {productId}";
            
            var queryResult = await db.ExecuteAsync(sql, productId);

            if (queryResult == -1)
                return false;
            return true;
        }
        public async Task DeleteProductById(int productId)
        {
            bool productExists = await ValidateProductExists(productId);  
            bool productInOrder = await ValidateProductInOrder(productId);

            if (!productExists)
                throw new Exception("the product doesn't exist");
            else if (productInOrder)
                throw new Exception("the product is in an order");
            else
                await db.ExecuteAsync($"Delete From Products Where ProductId = {productId}");
        }
        private async Task<bool> ValidateProductInOrder(int productId)
        {
            var sql = "Select * from Products p" +
                " Inner join Products_Orders po On po.ProductId = p.ProductId" +
                " Inner join Orders o On o.OrderId = po.OrderId" +
                $" Where p.ProductId = {productId}";

            var queryResult = await db.ExecuteAsync(sql, productId);
            
            if (queryResult == -1)
                return false;
            return true;
        }
    }
}
