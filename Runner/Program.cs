using TalanShoppingAccessLayer.BusinessLogic.BLOrders;
using TalanShoppingAccessLayer.BusinessLogic.BLProducts;
using TalanShoppingAccessLayer.Models;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Transactions;
using TalanUserAccessLayer.BusinessLogic;
using TalanUserAccessLayer.DTO;
using TalanUserAccessLayer.Models;

namespace Runner
{
    class Program
    {
        private static IConfigurationRoot config;

        static async Task Main(string[] args)
        {
            Initialize();

            //List<Task> tasks = new List<Task>();

            //tasks.Add(GetAllProducts_should_return_a_list_of_products());
            //tasks.Add(GetProductById_should_return_a_product());
            //Stopwatch stopwatch = Stopwatch.StartNew();
            //Task.WaitAll(tasks.ToArray());
            //stopwatch.Stop();
            //Console.WriteLine(stopwatch.Elapsed.ToString());
            //await AddProduct_should_add_a_product();
            //await UpdateProduct_should_update_a_product();
            //await DeleteProductById_Should_remove_a_product();

            //await GetAllOrders_should_return_a_list_of_orders();
            //await GetAllOrdersWithProducts_should_return_a_list_of_orders_with_products();
            //await GetOrderById_should_return_an_order();
            //await GetOrderByIdWithProducts_should_return_an_order_with_products();
            //await AddOrder_should_add_an_order_with_its_products();
            //await UpdateOrder_should_modify_an_existing_order();
            //await DeleteOrderById_should_remove_an_order();

            //await LogIn_should_return_Jean();
            await SignUp_should_return_Jeanne_user();
        }

        private static void Initialize ()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            config = builder.Build();
        }

        private static IProductBL CreateProductsBL()
        {
            return new ProductBL(config.GetConnectionString("ShoppingDBConnection"));
        }
        private static IOrdersBL CreateOrdersBL()
        {
            return new OrdersBL(config.GetConnectionString("ShoppingDBConnection"));
        }

        private static IUsersBL CreateUsersBL()
        {
            return new UsersBL(config.GetConnectionString("UsersDBConnection"));
        }

        static async Task GetAllProducts_should_return_a_list_of_products()
        {
            var repository = CreateProductsBL();

            // act 
            List<Product> products = await repository.GetAllProducts();

            // assert
            Console.WriteLine($"Count: {products.Count()}");
            products.Output();
            Debug.Assert(products.Count() > 6);
        }
        static async Task GetProductById_should_return_a_product()
        {
            var repository = CreateProductsBL();

            int productId = 1;

            // act 
            Product product = await repository.GetProductById(productId);

            // assert
            product.Output();
            Debug.Assert(product.Name != null);
     

        }
        static async Task AddProduct_should_add_a_product()
        {
            // arrange
            IProductBL repository = CreateProductsBL();
            var product = new Product
            {
                Name = "Bananes",
                Price = 11.99,
                Picture = "image de bananes",
                Description = ""
            };

            // act
            var productsBefore = await repository.GetAllProducts();
            await repository.AddProduct(product);
            var productsAfter = await repository.GetAllProducts();

            // assert

            productsAfter.Last().Output();
            Debug.Assert(productsAfter.Contains(product));
            Console.WriteLine($"products before add : {productsBefore.Count}");
            Console.WriteLine($"products after add : {productsAfter.Count}");
        }
        static async Task UpdateProduct_should_update_a_product()
        {
            // arrange
            IProductBL repository = CreateProductsBL();
            var product = new Product
            {
                ProductId = 1,
                Name = "Fraise",
                Price = 1.99,
                Picture = "image de fraises",
                Description = ""
            };

            // act
            await repository.UpdateProduct(product);
            var firstProduct = await repository.GetProductById(1);


            // assert
            firstProduct.Output();
            Debug.Assert(firstProduct.Picture != null);
        }
        static async Task DeleteProductById_Should_remove_a_product()
        {
            // arrange
            var repository = CreateProductsBL();
            int productId = 110;

            // act
            var productsBefore = await repository.GetAllProducts();
            await repository.DeleteProductById(productId);
            var productsAfter = await repository.GetAllProducts();

            // assert
            Debug.Assert(productsAfter.Count() < productsBefore.Count());
            Console.WriteLine($"products before delete : {productsBefore.Count}");
            Console.WriteLine($"products after delete : {productsAfter.Count}");
        }
        static async Task GetAllOrders_should_return_a_list_of_orders()
        {
            //arrange
            var repository = CreateOrdersBL();

            // act 
            List<Order> orders = await repository.GetAllOrders();

            // assert
            orders.Output();
            Debug.Assert(orders.Count > 0);
        }
        static async Task GetAllOrdersWithProducts_should_return_a_list_of_orders_with_products()
        {
            //arrange
            var repository = CreateOrdersBL();

            // act 
            List<Order> ordersWithProducts = await repository.GetAllOrdersWithProducts();

            // assert
            ordersWithProducts.Output();
            Debug.Assert(ordersWithProducts.Count > 0);
        }
        static async Task GetOrderById_should_return_an_order()
        {
            //arrange
            var repository = CreateOrdersBL();
            int orderId = 1;

            //act
            Order order = await repository.GetOrderById(orderId);

            //assert
            order.Output();
            Debug.Assert(order != null);
        }
        static async Task GetOrderByIdWithProducts_should_return_an_order_with_products()
        {
            //arrange
            var repository = CreateOrdersBL();
            int orderId = 1;

            //act
            Order order = await repository.GetOrderByIdWithProducts(orderId);

            //assert
            order.Output();
            Debug.Assert(order != null);
        }
        static async Task AddOrder_should_add_an_order_with_its_products()
        {
            //arrange
            var repository = CreateOrdersBL();
            var order = new Order
            {
                Name = "testOrder",
                ProductOrders = new List<ProductOrder>
                {
                    new ProductOrder { ProductId = 1, Quantity = 1 },
                    new ProductOrder { ProductId = 2, Quantity = 2 },
                    new ProductOrder { ProductId = 3, Quantity = 3 },
                }
            };

            // act
            var ordersBefore = await repository.GetAllOrders();
            await repository.AddOrder(order);
            var ordersAfter = await repository.GetAllOrders();

            // assert

            ordersAfter.Last().Output();
            Debug.Assert(ordersAfter.Last().NumberOfProducts == 6);
            Console.WriteLine($"products before add : {ordersBefore.Count}");
            Console.WriteLine($"products after add : {ordersAfter.Count}");
        }
        static async Task UpdateOrder_should_modify_an_existing_order()
        {
            //arrange
            var repository = CreateOrdersBL();
            var order = new Order
            {
                OrderId = 1,
                Name = "orderModified",
                ProductOrders = new List<ProductOrder>
                {
                    new ProductOrder { ProductId = 1, Quantity = 1 },
                    new ProductOrder { ProductId = 2, Quantity = 2 },
                    new ProductOrder { ProductId = 5, Quantity = 2 },
                }
            };

            // act
            await repository.UpdateOrder(order);
            Order modifiedOrder = await repository.GetOrderByIdWithProducts(1);

            // assert
            modifiedOrder.Output();
            Debug.Assert(modifiedOrder.NumberOfProducts == 5);
        }
        static async Task DeleteOrderById_should_remove_an_order()
        {
            // arrange
            var repository = CreateOrdersBL();
            int orderId = 10;

            // act
            var ordersBefore = await repository.GetAllOrders();
            await repository.DeleteOrderById(orderId);
            var ordersAfter = await repository.GetAllOrders();

            // assert
            Debug.Assert(ordersAfter.Count() < ordersBefore.Count());
            Console.WriteLine($"products before delete : {ordersBefore.Count}");
            Console.WriteLine($"products after delete : {ordersAfter.Count}");
        }

        static async Task LogIn_should_return_Jean()
        {
            var repository = CreateUsersBL();

            AuthenticationUser authenticationUser = new AuthenticationUser
            {
                Login = "jean.jean@gmail.com",
                Password = "jeanPassword"
            };

            AuthenticationUser badAuthentication = new AuthenticationUser
            {
                Login = "JEAN",
                Password = "badPassword"
            };

            // act 
            User user = await repository.LogIn(badAuthentication);

            // assert
            user.Output();
            Console.WriteLine(user);
            Debug.Assert(user.FirstName == "Jean");

        }

        static async Task SignUp_should_return_Jeanne_user()
        {
            var repository = CreateUsersBL();

            CreationUser creationUser = new CreationUser
            {
                FirstName = "Jeanne",
                LastName = "Jeanne",
                Email = "jeanne.jeanne@gmail.com",
                Username = "JEANNE",
                Password = "jeannePassword",
                Admin = false
            };

            // act 
            User user = await repository.SignUp(creationUser);

            // assert
            user.Output();
            Console.WriteLine(user);
            Debug.Assert(user.FirstName == "Jeanne");

        }

    }
}