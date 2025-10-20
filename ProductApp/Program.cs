// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using ProductConsoleApp;

Console.WriteLine("Hello, World!");
ProductService productService = new ProductService();
var lst = await productService.GetProducts();
foreach (var item in lst)
{
    Console.WriteLine(item.ProductName);
    Console.WriteLine( item.Price);
}

var newProduct = await productService.CreateProduct(new ProductService.ProductCreateRequestModel
{
    
    ProductCode = "P004",
    ProductName = "Banana",
    Price = 1000,
    Quantity = 20
});
Console.WriteLine(newProduct.Message);

