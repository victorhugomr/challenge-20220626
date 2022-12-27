using Microsoft.Extensions.Options;
using MongoDB.Driver;
using challenge_20220626.Models;

namespace challenge_20220626.Services{
    public class ProductServices{
        private readonly IMongoCollection<Product> _productCollection;

        public ProductServices(IOptions<ProductDatabaseSettings> productServices){

            var mongoClient = new MongoClient(productServices.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(productServices.Value.DatabaseName);

            _productCollection = mongoDatabase.GetCollection<Product>
                (productServices.Value.productCollectionName);
        }

        public async Task<List<Product>> GetAsync() =>
            await _productCollection.Find(x => true).ToListAsync();
        public async Task<Product> GetAsync(long code) =>
           await _productCollection.Find(x => x.code == code).FirstOrDefaultAsync();
        public async Task CreateAsync(Product product) =>
            await _productCollection.InsertOneAsync(product);
        public async Task UpdateAsync(string id, Product product) =>
           await _productCollection.ReplaceOneAsync(x => x.id == id, product);
        public async Task RemoveAsync(string id) =>
            await _productCollection.DeleteOneAsync(x => x.id == id);

    }
}