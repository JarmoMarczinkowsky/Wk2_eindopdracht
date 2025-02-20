using Microsoft.Identity.Client;
using EindOpdracht.WebApi.Services;
using Microsoft.AspNetCore.Identity;
using System.Data.Common;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.Configure<RouteOptions>(o => o.LowercaseUrls = true);

var sqlConnectionString = builder.Configuration.GetValue<string>("SqlConnectionString"); 
var sqlConnectionStringFound = !string.IsNullOrWhiteSpace(sqlConnectionString);

builder.Services.AddTransient<SqlEnvironment2DRepository, SqlEnvironment2DRepository>(o => new SqlEnvironment2DRepository(sqlConnectionString));

builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<IdentityUser>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequiredLength = 50;
})
.AddRoles<IdentityRole>()
.AddDapperStores(options =>
{
    options.ConnectionString = sqlConnectionString;
});

var app = builder.Build();
app.UseAuthorization();

app.MapGroup("/account").MapIdentityApi<IdentityUser>();

app.MapGet("/", () => $"The API is up 🛸\nConnection string found: {(sqlConnectionStringFound ? "✅ hij doet het 👍👍👍👍" : "❌")}");

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.MapOpenApi();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers().RequireAuthorization();

app.Run();
