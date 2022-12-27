using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using challenge_20220626.Models;
using challenge_20220626.Services;

namespace challenge_20220626.Controllers
{
    [Route("/")]
    [ApiController]
    public class ProductController : ControllerBase{
        private readonly ProductServices _productServices;

        public ProductController(ProductServices productServices){
            _productServices = productServices;
        }

        [HttpGet]
        public string GetStatus()
            => "Fullstack Challenge 20201026";
        
        [HttpGet]
        [Route("products")]
        public async Task<List<Product>> GetProducts()
            => await _productServices.GetAsync();

        [HttpGet]
        [Route("products/{code}")]
        public async Task<Product> GetProduct(long code)
            => await _productServices.GetAsync(code);

    }
}