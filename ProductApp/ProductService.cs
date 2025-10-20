using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ProductConsoleApp;

public class ProductService
{
    private readonly string domainUrl = "https://localhost:7055";
    private readonly string productEndpoint = "/api/product";
    private readonly HttpClient _client;

    public ProductService()
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri(domainUrl);
    }

    public async Task<List<ProductModel>> GetProducts()
    {
        var response = await _client.GetAsync(productEndpoint);

        if (response.IsSuccessStatusCode)
        {
            var jsonStr = await response.Content.ReadAsStringAsync();
            var lst = JsonConvert.DeserializeObject<List<ProductModel>>(jsonStr);

            return lst;
        }
        return new List<ProductModel>();
    }
    public async Task<ProductCreateResponseModel> CreateProduct(ProductCreateRequestModel request)
    {
        var jsonStr = JsonConvert.SerializeObject(request);
        StringContent content = new StringContent(jsonStr,Encoding.UTF8, Application.Json);

        var response = await _client.PostAsync(productEndpoint, content);
     
        
            var json = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<ProductCreateResponseModel>(json);  
            return model?? new ProductCreateResponseModel();
        
        
    }

    public class ProductCreateRequestModel
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
    public class ProductCreateResponseModel
    {
        public bool IsSuccess { get; set; } 
        public string Message { get; set; }
    }
    public class ProductModel
    {
        public string ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool DeleteFlag { get; set; }
    }
}
