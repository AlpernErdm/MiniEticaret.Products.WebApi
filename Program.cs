using Microsoft.EntityFrameworkCore;
using MiniEticaret.Products.WebApi.Context;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
