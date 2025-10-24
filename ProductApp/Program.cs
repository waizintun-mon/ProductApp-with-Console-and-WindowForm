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

//var newProduct = await productService.CreateProduct(new ProductService.ProductCreateRequestModel
//{

//    ProductCode = "P004",
//    ProductName = "Banana",
//    Price = 1000,
//    Quantity = 20
//});
//Console.WriteLine(newProduct.Message);
ProductRestService restService = new ProductRestService();  
//var updateProduct = await restService.UpdateProduct(new ProductService.ProductUpdateRequestModel
//{
//   ProductId = "b7398068-035b-4283-a8d9-f9f094522b6d",
//  ProductCode ="ABC123",
//  ProductName = "Test Product",
//  Price = 1000,
//  Quantity = 10

//}); 
//Console.WriteLine(updateProduct.Message);   
var deleteProduct = await restService.DeleteProduct(new ProductService.ProductDeleteRequestModel
{
    //ProductId = "e4134732-11aa-4691-8433-640245a80fe2",
    ProductId = "33959c80-7ed8-4ef5-859e-f68b6116bb49"
});
Console.WriteLine(deleteProduct.Message);


