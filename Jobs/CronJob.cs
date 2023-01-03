using HtmlAgilityPack;
using challenge_20220626.Models;
using challenge_20220626.Services;

using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace challenge_20220626.Jobs{
    public class CronJob : IJob{

        private readonly ProductServices _productServices;
        private readonly ILogger<CronJob> _logger;

        public CronJob(ProductServices productServices, ILogger<CronJob> logger)
        {
            this._productServices = productServices;
            this._logger = logger;
        }
        public Task Execute(IJobExecutionContext context)
        {
            WebScraper();
            return Task.CompletedTask;
        }
        
        static HtmlDocument GetDocument(string url){
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);
            return doc;
        }

        static List<string> GetProductLinks(string url){
            var doc = GetDocument(url);
            var linkNodes = doc.DocumentNode.SelectNodes("//body/div/div[2]/div[2]/div/div/div/div[6]/div/div/ul/li/a");
            var baseUri = new Uri(url);
            var links = new List<string>();

            foreach(var node in linkNodes){
                var link = node.Attributes["href"].Value;
                link = new Uri(baseUri, link).AbsoluteUri;
                links.Add(link);
            }
            return links;
        }

        private static List<Product> GetProducts(List<string> links){
            var products = new List<Product>();
            //Limitar a importação a somente 100 produtos;
            foreach(var link in links.Take(100)){
                var product = new Product();
                var doc = GetDocument(link);
                
                string xpath = "//*[@id=\"barcode\"]";
                //Diferencial 2 Configurar um sistema de alerta se tem algum falho durante o Sync dos produtos;
                var test = doc.DocumentNode.SelectSingleNode(xpath);
                if(test != null){
                    product.code = long.Parse(doc.DocumentNode.SelectSingleNode(xpath).InnerText);
                    Console.WriteLine("code: {0}", product.code);
                }
                else{
                    Console.WriteLine("Falha na sincronização de code");
                }
                xpath = "//*[@id=\"barcode_paragraph\"]/text()[2]";
                test = doc.DocumentNode.SelectSingleNode(xpath);
                if(test != null){
                    product.barcode = product.code + doc.DocumentNode.SelectSingleNode(xpath).InnerText;
                    Console.WriteLine("barcode: {0}", product.barcode);
                }
                else{
                    Console.WriteLine("Falha na sincronização de barcode");
                }
                //Todos os produtos deverão ter os campos personalizados imported_t e status
                product.status = "imported";
                Console.WriteLine("status: {0}", product.status);
                product.importedT = DateTime.UtcNow.ToString("s") + "Z";
                Console.WriteLine("datetime: {0}", product.importedT);
                product.url = "https://world.openfoodfacts.org/product/" + product.code;
                Console.WriteLine("url: {0}", product.url);
                xpath = "//*[@id=\"prodHead\"]/div/div/h1";
                test = doc.DocumentNode.SelectSingleNode(xpath);
                if(test != null){
                    product.productName = doc.DocumentNode.SelectSingleNode(xpath).InnerText;
                    Console.WriteLine("productName: {0}", product.productName);
                }
                else{
                    Console.WriteLine("Falha na sincronização de productName");
                }
                xpath = "//*[@id=\"field_quantity_value\"]";
                test = doc.DocumentNode.SelectSingleNode(xpath);
                if(test != null){
                    product.quantity = doc.DocumentNode.SelectSingleNode(xpath).InnerText;
                    Console.WriteLine("quantity: {0}", product.quantity);
                }
                else{
                    Console.WriteLine("Falha na sincronização de quantity");
                }
                xpath = "//*[@id=\"field_categories\"]";
                test = doc.DocumentNode.SelectSingleNode(xpath);
                if(test != null){
                    var test2 = doc.DocumentNode.SelectSingleNode(xpath).InnerText;
                    product.categories = test2.Split(':')[1];
                    Console.WriteLine("categories: {0}", product.categories);
                }
                else{
                    Console.WriteLine("Falha na sincronização de categories");
                }
                xpath = "//*[@id=\"field_packaging_value\"]";
                test = doc.DocumentNode.SelectSingleNode(xpath);
                if(test != null){
                    product.packaging = doc.DocumentNode.SelectSingleNode(xpath).InnerText;
                    Console.WriteLine("packaging: {0}", product.packaging);
                }
                else{
                    Console.WriteLine("Falha na sincronização de packaging");
                }
                xpath = "//*[@id=\"field_brands_value\"]";
                test = doc.DocumentNode.SelectSingleNode(xpath);
                if(test != null){
                    product.brands = doc.DocumentNode.SelectSingleNode(xpath).InnerText;
                    Console.WriteLine("brands: {0}", product.brands);
                }
                else{
                    Console.WriteLine("Falha na sincronização de brands");
                }
                xpath = "//*[@id=\"og_image\"]";
                test = doc.DocumentNode.SelectSingleNode(xpath);
                if(test != null){
                    product.imageUrl = doc.DocumentNode.SelectSingleNode(xpath).Attributes["src"].Value;
                    Console.WriteLine("imageUrl: {0}", product.imageUrl);
                }
                else{
                    Console.WriteLine("Falha na sincronização de imageUrl");
                }
                Console.WriteLine();
                products.Add(product);
            }
            return products;
        }

        public async Task<List<Product>> UpdateDatabase(string url){
            var links = GetProductLinks(url);
            List<Product> products = GetProducts(links);
            List<Product> productsDB = await _productServices.GetAll();
            List<Product> updateDatabase = new List<Product>();

            if(productsDB.Count == 0){
                return products;
            }
            else{
                if(productsDB.Count > 0){
                foreach(var product in products){
                    int flag = 0;
                    foreach(var productDB in productsDB){
                        if(product.code == productDB.code){
                            flag = 1;
                        }
                    }
                    if(flag == 0){
                        updateDatabase.Add(product);
                    }
                }
            }
            return updateDatabase;
            }
        }

        public async void WebScraper(){
            string url = "https://world.openfoodfacts.org/";
            List<Product> updateDatabase = await UpdateDatabase(url);

            if(updateDatabase.Count != 0){
                await _productServices.CreateMany(updateDatabase);
            }
        }
    }
}