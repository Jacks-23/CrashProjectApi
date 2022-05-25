using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TalanShoppingAccessLayer.Models;

namespace TalanShoppingAccessLayer.BusinessLogic.BLOrders
{
    public class OrdersBL : IOrdersBL
    {
        private IDbConnection db;

        public OrdersBL(string connectionString)
        {
            db = new SqlConnection(connectionString);
        }

        public async Task<List<Order>> GetAllOrders()
        {
            var sql = "Select OrderId, Name, DateCrea, Total, NumberOfProducts" +
                " from Orders";

            var queryResult = await db.QueryAsync<Order>(sql);

            List<Order> orders = queryResult.ToList();

            return orders;
        }
        public async Task<List<Order>> GetAllOrdersWithProducts()
        {
            var sql = "Select o.OrderId, o.Name, o.DateCrea, o.Total, o.NumberOfProducts," +
                " p.ProductId, p.Name, p.Price, p.Description, p.Picture" +
                " From Orders o" +
                " Inner Join Products_Orders po On po.OrderId = o.OrderId" +
                " Inner Join Products p On p.ProductId = po.ProductId" +
                " Order by o.OrderId";

            var queryResult = await db.QueryAsync<Order, Product, Order>(sql, (order, product) =>
            {
                order.Products.Add(product);
                return order;
            }, splitOn: "OrderId,ProductId");

            var groupedResult = queryResult.GroupBy(o => o.OrderId).Select(g =>
            {
                var groupedOrder = g.First();
                groupedOrder.Products = g.Select(o => o.Products.Single()).ToList();
                return groupedOrder;
            });

            List<Order> ordersWithProducts = groupedResult.ToList();

            return ordersWithProducts;
        }
        public async Task<Order> GetOrderById(int orderId)
        {
            var sql = "Select OrderId, Name, DateCrea, Total, NumberOfProducts" +
                " from Orders" +
                $" Where OrderId = {orderId}";
            
            var queryResult = await db.QueryAsync<Order>(sql, orderId);

            if (!queryResult.Any())
                throw new Exception("No order found");

            Order order = queryResult.First();

            return order;
          
        }
        public async Task<Order> GetOrderByIdWithProducts(int orderId)
        {
            var sql = "Select o.OrderId, o.Name, o.DateCrea, o.Total, o.NumberOfProducts," +
                " p.ProductId, p.Name, p.Price, p.Description, p.Picture" +
                " From Orders o" +
                " Inner Join Products_Orders po On po.OrderId = o.OrderId" +
                " Inner Join Products p On p.ProductId = po.ProductId" +
                $" Where o.OrderId = {orderId}";

            var queryResult = await db.QueryAsync<Order, Product, Order>(sql, (order, product) =>
            {
                order.Products.Add(product);
                return order;
            }, splitOn: "OrderId,ProductId");

            if (!queryResult.Any())
                throw new Exception("No order found");

            var groupedResult = queryResult.GroupBy(o => o.OrderId).Select(o =>
            {
                var grouped = o.First();
                grouped.Products = o.Select(o => o.Products.Single()).ToList();
                return grouped;
            });

            Order orderWithProducts = groupedResult.First();

            return orderWithProducts;
        }
        public async Task AddOrder(Order order)
        {
            db.Open();
            var transaction = db.BeginTransaction(System.Data.IsolationLevel.Serializable);
            try
            {
                if (order.Name == "")
                    throw new Exception("An order needs a name");
                if (order.ProductOrders.Count == 0)
                    throw new Exception("Your order has no products");
      
                (double total, int numberOfProducts) = CalculateTotalAndNumberOfProductsOfAnOrder(order, transaction);
                
                var sql = "INSERT INTO Orders (Name, Total, NumberOfProducts)" +
                    $" VALUES(@Name, {total.ToString("F2", CultureInfo.InvariantCulture)}, {numberOfProducts})" +
                    " Select Cast(Scope_Identity() as int)";

                db.Execute(sql, order, transaction: transaction);
                
                await LinkOrderWithProducts(order, transaction);
                
                transaction.Commit();
                db.Close();

            }
            catch (TransactionAbortedException e)
            {
                transaction.Rollback();
                throw new Exception (e.Message);
            }
 
        }
        private Tuple<double,int> CalculateTotalAndNumberOfProductsOfAnOrder(Order order, IDbTransaction transaction)
        {
            double total = 0;
            int numberOfProducts = 0;

            List<int> productsIds = order.ProductOrders.Select(o => o.ProductId).ToList();
            string productsIdsForQuery = string.Join(",", productsIds);

            var sql = "Select [ProductId], [Name], [Price], [Picture], [Description]" +
                " From Products" +
                $" Where ProductId in ({productsIdsForQuery})";
            
            var queryResult = db.Query<Product>(sql, transaction: transaction);
            List<Product> products = queryResult.ToList();

            foreach (var productOrder in order.ProductOrders)
            {
                Product product = products.Where(p => p.ProductId == productOrder.ProductId).First();
                total += product.Price * productOrder.Quantity;
                numberOfProducts += productOrder.Quantity;
            }

            return Tuple.Create(total, numberOfProducts);
        }
        private async Task LinkOrderWithProducts(Order order, IDbTransaction transaction)
        {
            var sql = "Select MAX(OrderId) from Orders";

            var queryResult = db.Query<int>(sql, transaction: transaction);

            int lastOrderId = queryResult.First();

            foreach (var productOrders in order.ProductOrders)
            {
                productOrders.OrderId = lastOrderId;
                await AddProductOrder(productOrders, transaction);
            }
        }
        private async Task AddProductOrder(ProductOrder productOrder, IDbTransaction transaction)
        {
            var sql = "INSERT INTO [dbo].[Products_Orders]" +
                "VALUES (@ProductId, @OrderId, @Quantity)";

            await db.ExecuteAsync(sql, productOrder, transaction: transaction);
        }
        public async Task UpdateOrder(Order order)
        {
            db.Open();
            var transaction = db.BeginTransaction(System.Data.IsolationLevel.Serializable);

            try
            {
                bool orderExists = ValidateOrderExists(order.OrderId, transaction);

                if (!orderExists)
                    throw new Exception("the order doesn't exist");
                else if (order.Name == "")
                    throw new Exception("An order needs a name");
                else if (order.ProductOrders.Count == 0)
                    throw new Exception("Your order has no products");

                (double total, int numberOfProducts) = CalculateTotalAndNumberOfProductsOfAnOrder(order, transaction);

                var sql = "Update Orders Set" +
                " Name = @Name," +
                $" Total = {total.ToString("F2", CultureInfo.InvariantCulture)}," +
                $" NumberOfProducts = {numberOfProducts}" +
                " Where OrderId = @OrderId";

                db.Execute(sql, order, transaction: transaction);

                await UpdateLinksBetweenOrderAndProducts(order, transaction);

                transaction.Commit();
                db.Close();

            }
            catch (TransactionAbortedException e)
            {
                transaction.Rollback();
                throw new Exception(e.Message);
            }
        }
        private bool ValidateOrderExists(int orderId, IDbTransaction? transaction)
        {
            var sql = "Select [OrderId]" +
                " From Orders" +
                $" Where OrderId = {orderId}";

            var queryResult = db.ExecuteScalar<int>(sql, orderId, transaction: transaction);

            if (queryResult == 0)
                return false;
            return true;
        }
        private async Task UpdateLinksBetweenOrderAndProducts(Order order, IDbTransaction transaction)
        {
            DeleteProductsOrder(order.OrderId, transaction);

            foreach (var productOrders in order.ProductOrders)
            {
                productOrders.OrderId = order.OrderId;
                await AddProductOrder(productOrders, transaction);
            }
        }
        private void DeleteProductsOrder(int orderId, IDbTransaction transaction)
        {
            var sql = $"Delete From Products_Orders Where OrderId = {orderId}";
            db.Execute(sql, orderId, transaction: transaction);
        }
        public async Task DeleteOrderById(int orderId)
        {
            bool orderExists = ValidateOrderExists(orderId, null);

            if (!orderExists)
                throw new Exception("the order doesn't exist");

            var sql = $"Delete From Orders Where OrderId = {orderId}";
            await db.ExecuteAsync(sql);
        }

    }
}
