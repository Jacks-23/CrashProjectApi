using TalanShoppingAccessLayer.BusinessLogic.BLOrders;
using TalanShoppingAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;

namespace TalanAPICrashProject.ApiEndpoints
{
    
    public static class ApiOrders
    {
        private static IConfiguration config;

        
        public static void ConfigureApiOrders(this WebApplication app)
        {
            Initialize(app);
            // All of my API endpoint mapping
            app.MapGet("/Orders", ApiGetAllOrdersWithProducts).RequireAuthorization();
            app.MapGet("/Orders/{id}", ApiGetOrderByIdWithProducts).AllowAnonymous();
            app.MapPost("/Orders", ApiAddOrder).AllowAnonymous();
            app.MapPut("/Orders", ApiUpdateOrder).AllowAnonymous();
            app.MapDelete("/Orders", ApiDeleteOrderById).AllowAnonymous();
        }

        private static void Initialize(this WebApplication app)
        {
            config = app.Configuration;
        }


        private static IOrdersBL CreateOrdersBL()
        {
            return new OrdersBL(config.GetConnectionString("ShoppingDBConnection"));
        }

        private static async Task<IResult> ApiGetAllOrdersWithProducts()
        {
            var repository = CreateOrdersBL();

            try
            {
                return Results.Ok(await repository.GetAllOrdersWithProducts());
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }

        }

        private static async Task<IResult> ApiGetOrderByIdWithProducts(int orderId)
        {
            var repository = CreateOrdersBL();

            try
            {
                return Results.Ok(await repository.GetOrderByIdWithProducts(orderId));
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }
        }

        private static async Task<IResult> ApiAddOrder(Order order)
        {
            var repository = CreateOrdersBL();

            try
            {
                await repository.AddOrder(order);
                return Results.Ok("Order added successfully");
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }

        }
        private static async Task<IResult> ApiUpdateOrder(Order order)
        {
            var repository = CreateOrdersBL();

            try
            {
                await repository.UpdateOrder(order);
                return Results.Ok("Order updated successfully");
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }

        }

        private static async Task<IResult> ApiDeleteOrderById(int orderId)
        {
            var repository = CreateOrdersBL();

            try
            {
                await repository.DeleteOrderById(orderId);
                return Results.Ok("Order deleted");

            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }
    }
}
