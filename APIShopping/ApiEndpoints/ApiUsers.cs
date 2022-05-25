using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TalanAPICrashProject.Models;
using TalanUserAccessLayer.BusinessLogic;
using TalanUserAccessLayer.DTO;
using TalanUserAccessLayer.Models;

namespace TalanAPICrashProject.ApiEndpoints
{
    
    public static class ApiUsers
    {
        private static IConfiguration config;
        
        public static void ConfigureApiUsers(this WebApplication app)
        {
            Initialize(app);
            app.MapPost("/Users/LogIn", APILogIn).AllowAnonymous();
            app.MapPost("/Users/SignUp", APISignUp).AllowAnonymous();
        }

        private static void Initialize(this WebApplication app)
        {
            config = app.Configuration;
        }

        
        private static IUsersBL CreateUsersBL()
        {
            return new UsersBL(config.GetConnectionString("UsersDBConnection"));
        }

        private static async Task<IResult> APILogIn(AuthenticationUser authenticationUser)
        {
            try
            {
                var repository = CreateUsersBL();

                User user = await repository.LogIn(authenticationUser);
                string token = GenerateApiToken(user);
                UserWithToken userWithToken = AddTokenToUser(user, token);

                return Results.Ok(userWithToken);
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }
        }
        private static string GenerateApiToken(User user)
        {
            string mySecret = "asdfSFS34wfsdfsdfSDSD32dfsddDDerQSNCK34SOWEK5354fdgdf4";
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret));
            string tokenIssuer = "http://localhost:3000";
            string tokenAudience = "https://localhost:7244";
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.FirstName)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                Issuer = tokenIssuer,
                Audience = tokenAudience,
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            string stringToken = tokenHandler.WriteToken(securityToken);
            return stringToken;

        }
        private static UserWithToken AddTokenToUser(User user, string token)
        {
            var userWithToken = new UserWithToken
            {
                User = user,
                Token = token,
            };
            
            return userWithToken;
        }

        private static async Task<IResult> APISignUp(CreationUser creationUser)
        {
            try
            {
                var repository = CreateUsersBL();

                User user = await repository.SignUp(creationUser);
                string token = GenerateApiToken(user);
                UserWithToken userWithToken = AddTokenToUser(user, token);

                return Results.Ok(userWithToken);
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }
        }

    }
}
