using Azure;
using Bogus;
using Microsoft.EntityFrameworkCore;
using MiniEticaret.Products.WebApi.Context;
using MiniEticaret.Products.WebApi.Dtos;
using MiniEticaret.Products.WebApi.Models;
using TS.Result;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/seedData", (ApplicationDbContext dbContext) =>
{
    for (int i = 0; i < 100; i++)
    {
        Faker faker = new();
        Product product = new()
        {
            Name = faker.Commerce.ProductName(),
            Price = Convert.ToDecimal(faker.Commerce.Price()),
            Stock = faker.Commerce.Random.Int(1, 100)
        };
        dbContext.Products.Add(product);
    }
    dbContext.SaveChanges();
    return Results.Ok(Result<string>.Succeed("Seed Data baþarýyla oluþturuldu"));
});
app.MapGet("/getALl", async (ApplicationDbContext dbContext, CancellationToken cancellationToken) =>
{
    var products = await dbContext.Products.OrderBy(p => p.Name).ToListAsync(cancellationToken);
    Result<List<Product>> response = products;
    return response;
});
app.MapPost("/create", async(CreateProductDto request,ApplicationDbContext dbContext, CancellationToken cancellationToken) =>
{
    bool isNameExist = await dbContext.Products.AnyAsync(p => p.Name == request.Name, cancellationToken);
    if (isNameExist)
    {
        var response = Result<string>.Failure("Ürün adý daha önce oluþturulmuþ");
        return Results.BadRequest(response);
    }
    Product product = new()
    {
        Name = request.Name,
        Price = request.Price,
        Stock = request.Stock
    };
    await dbContext.AddAsync(product, cancellationToken);
    await dbContext.SaveChangesAsync(cancellationToken);

    return Results.Ok(Result<string>.Succeed("Ürün kaydý baþarýyla tamamlandý"));
});

using (var scoped = app.Services.CreateScope()) //auto migrate atar
{
    var srv=scoped.ServiceProvider;
    var context=srv.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
}
app.Run();
