namespace MiniEticaret.Products.WebApi.Dtos
{
    public record CreateProductDto(
         string Name,
         decimal Price, 
         int Stock
    );
}
