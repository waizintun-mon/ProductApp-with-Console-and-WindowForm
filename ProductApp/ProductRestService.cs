using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProductConsoleApp.ProductService;

namespace ProductConsoleApp
{
    public class ProductRestService
    {
        private readonly RestClient _restClient;
        private readonly string domainUrl = "https://localhost:7055";
        private readonly string productEndpoint = "/api/product";

        public ProductRestService()
        {
            _restClient = new RestClient(domainUrl);
        }
        public async Task<List<ProductModel>> GetProducts()
        {
            RestRequest restRequest = new RestRequest(productEndpoint,Method.Get);
            var response = await _restClient.GetAsync(restRequest);
            if (response.IsSuccessStatusCode)
            {
                var jsonStr = response.Content;
                var model = JsonConvert.DeserializeObject<List<ProductModel>>(jsonStr)!;
                return model;
            }
            return new List<ProductModel>();    
        }
        public async Task<ProductCreateResponseModel> CreateProduct (ProductCreateRequestModel request)
        {
            RestRequest restRequest = new RestRequest(productEndpoint,Method.Post); 
            restRequest.AddJsonBody(request);
            var response = await _restClient.PostAsync(restRequest);
            var json = response.Content;
            var model = JsonConvert.DeserializeObject<ProductCreateResponseModel>(json);    
            return model ?? new ProductCreateResponseModel();   
        }
        public async Task<ProductUpdateResponseModel> UpdateProduct(ProductUpdateRequestModel request)
        {
            RestRequest restRequest = new RestRequest($"{productEndpoint }/{request.ProductId}", Method.Patch);
            var jsonStr = restRequest.AddJsonBody(request);
            var response = await _restClient.PatchAsync(jsonStr);
            Console.WriteLine($"Status: {response.StatusCode}, Content: {response.Content}");
            if (response == null)
            {
                throw new ApplicationException("No response received from server.");
            }
            if (!response.IsSuccessful)
                throw new Exception($"Error: {response.StatusCode} - {response.Content}");
            var json = response.Content;
            var model = JsonConvert.DeserializeObject<ProductUpdateResponseModel>(json);
            return model ?? new ProductUpdateResponseModel();   
        }
        public async Task<ProductDeleteResponseModel> DeleteProduct(ProductDeleteRequestModel request)
        {
            RestRequest restRequest = new RestRequest($"{productEndpoint}/{request.ProductId}", Method.Delete);
            var response = await _restClient.DeleteAsync(restRequest);
            var json = response.Content; 
            var model = JsonConvert.DeserializeObject<ProductDeleteResponseModel>(json);
            return model?? new ProductDeleteResponseModel();

        }
    }
 }

