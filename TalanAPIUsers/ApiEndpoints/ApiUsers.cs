using TalanUserAccessLayer.BusinessLogic;
using TalanUserAccessLayer.DTO;
using TalanUserAccessLayer.Models;

namespace TalanAPIUsers.ApiEndpoints
{
    public static class ApiUsers
    {
        private static IConfiguration config;

        public static void ConfigureApiUsers(this WebApplication app)
        {
            Initialize(app);
            app.MapPost("/Users/FindUser", APIFindUser);
            app.MapPost("/Users/AddUser", APIAddUser);
        }

        private static void Initialize(this WebApplication app)
        {
            config = app.Configuration;
        }

        private static IUsersBL CreateUsersBL()
        {
            return new UsersBL(config.GetConnectionString("UsersInfoDBConnection"));
        }

        private static async Task<IResult> APIFindUser(AuthenticationUser authenticationUser)
        {
            try
            {
                var repository = CreateUsersBL();

                User user = await repository.FindUser(authenticationUser);
                
                return Results.Ok(user);
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }
        }

        private static async Task<IResult> APIAddUser(CreationUser creationUser)
        {
            try
            {
                var repository = CreateUsersBL();

                await repository.AddUser(creationUser);

                return Results.Ok("User created");
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }
        }

    }
}
