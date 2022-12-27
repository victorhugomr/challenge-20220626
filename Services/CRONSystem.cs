using CsvHelper;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using challenge_20220626.Models;

namespace challenge_20220626.Services{
    public class CRONSystem{

        static HtmlDocument GetDocument(string url){
            var web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);
            return doc;
        }

        static List<string> GetProductLinks(string url){
            var doc = GetDocument(url);
            var linkNodes = doc.DocumentNode.SelectNodes("//li/a");

            var baseUri = new Uri(url);
            var links = new List<string>();
            foreach (var node in linkNodes){
                var link = node.Attributes["href"].Value;
                link = new Uri(baseUri, link).AbsoluteUri;
                links.Add(link);
            }
            return links;
        }

        private static List<Product> GetProducts(List<string> links){
            var products = new List<Product>();
            foreach (var link in links){
                var doc = GetDocument(link);
                var product = new Product();
                product.code = long.Parse(doc.DocumentNode.SelectSingleNode("span#barcode").InnerText);
                product.barcode = doc.DocumentNode.SelectSingleNode("barcode_paragraph").InnerText;
                product.status = "imported";
                //product.importedT = "2020-02-07T16:00:00Z"
                product.url = "https://world.openfoodfacts.org/product/{product.code}";
                var xpath = "//*[@id=\"product\"]/div/div/div[2]/div/div[2]/h2";
                product.productName = doc.DocumentNode.SelectSingleNode(xpath).InnerText;
                xpath = "//*[@id=\"field_quantity_value\"]";
                product.quantity = doc.DocumentNode.SelectSingleNode(xpath).InnerText;
                //product.categories = ;
                xpath = "//*[@id=\"field_packaging_value\"]";
                product.packaging = doc.DocumentNode.SelectSingleNode(xpath).InnerText;
                xpath = "//*[@id=\"field_brands_value\"]";
                product.brands = doc.DocumentNode.SelectSingleNode(xpath).InnerText;
                //product.imageUrl = ;

                products.Add(product);
            }
            return products;
        }
        
        private static void Export(List<Product> products){
            using (var writer = new StreamWriter("./products.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)){
                csv.WriteRecords(products);
            }

        }

        static void WebScraper(){
            string url = "https://world.openfoodfacts.org/";
            var links = GetProductLinks(url);
            List<Product> products = GetProducts(links);
            Export(products);
            // Console.Write(doc.DocumentNode.InnerHtml);
        }
    }
}