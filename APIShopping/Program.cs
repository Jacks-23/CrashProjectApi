using TalanAPICrashProject;
using TalanAPICrashProject.ApiEndpoints;
using TalanShoppingAccessLayer.BusinessLogic.BLOrders;
using TalanShoppingAccessLayer.BusinessLogic.BLProducts;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(auth =>
{
    auth.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(auth =>
    {
        auth.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = "http://localhost:3000",
            ValidAudience = "https://localhost:7244",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(
                "asdfSFS34wfsdfsdfSDSD32dfsddDDerQSNCK34SOWEK5354fdgdf4"))
        };
    });

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
}

app.UseAuthentication();
app.UseAuthorization();

app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseHttpsRedirection();


app.ConfigureApiProducts();

app.ConfigureApiOrders();
app.ConfigureApiUsers();

app.Run();