using TalanShoppingAccessLayer.BusinessLogic.BLProducts;
using TalanShoppingAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;

namespace TalanAPICrashProject.ApiEndpoints
{
    public static class ApiProducts
    {
        private static IConfiguration config;
        public static void ConfigureApiProducts(this WebApplication app)
        {
            Initialize(app);
            // All of my API endpoint mapping
            app.MapGet("/Products", ApiGetAllProducts).AllowAnonymous();
            app.MapGet("/Products/{id}", ApiGetProductById).AllowAnonymous();
            app.MapPost("/Products", ApiAddProduct).AllowAnonymous();
            app.MapPut("/Products", ApiUpdateProduct).AllowAnonymous();
            app.MapDelete("/Products", ApiDeleteProductById).AllowAnonymous();
        }

        private static void Initialize(this WebApplication app)
        {
            config = app.Configuration;
        }


        private static IProductBL CreateProductsBL()
        {
            return new ProductBL(config.GetConnectionString("ShoppingDBConnection"));
        }

        private static async Task<IResult> ApiGetAllProducts()
        {
            var repository = CreateProductsBL();

            try
            {
                return Results.Ok(await repository.GetAllProducts());
            }
            catch (Exception e)
            {

                return Results.Problem(e.Message);
            }

        }

        private static async Task<IResult> ApiGetProductById(int productId)
        {
            var repository = CreateProductsBL();

            try
            {
                return Results.Ok(await repository.GetProductById(productId));     
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }
        }

        private static async Task<IResult> ApiAddProduct(Product product)
        {
            var repository = CreateProductsBL();

            try
            {
                await repository.AddProduct(product);
                return Results.Ok();
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }
            
        }

        private static async Task<IResult> ApiUpdateProduct(Product product)
        {
            var repository = CreateProductsBL();

            try
            {
                await repository.UpdateProduct(product);
                return Results.Ok();
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }

        }

        private static async Task<IResult> ApiDeleteProductById(int productId)
        {
            var repository = CreateProductsBL();

            try
            {
                await repository.DeleteProductById(productId);
                return Results.Ok();
                    
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }
    }
}
