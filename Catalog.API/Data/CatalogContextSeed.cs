using Catalog.API.Entities;
using MongoDB.Driver;
using System.Collections.Generic;

namespace Catalog.API.Data
{
    public class CatalogContextSeed
    {
        public static void SeedData(IMongoCollection<Product> productCollection)
        {
            bool existProduct = productCollection.Find(p => true).Any();
            if(!existProduct)
            {
                productCollection.InsertManyAsync(GetMyProducts());
            }
        }

        private static IEnumerable<Product> GetMyProducts()
        {
            return new List<Product>()
            {
                new Product()
                {
                    Id = "602d214e773f2a3990b47ff5",
                    Name = "Iphone X",
                    Description = "Descrição do Iphone",
                    Image = "product-1.png",
                    Price = 950.00m,
                    Category = "Smartphone"
                },
                 new Product()
                {
                    Id = "602d214e773f2a3990b47ff6",
                    Name = "Samsung 10",
                    Description = "Descrição do Samsung",
                    Image = "product-2.png",
                    Price = 850.00m,
                    Category = "Smartphone"
                },
                 new Product()
                {
                    Id = "602d214e773f2a3990b47ff7",
                    Name = "Huawei Plus",
                    Description = "Descrição do Huawei",
                    Image = "product-3.png",
                    Price = 849.90m,
                    Category = "Smartphone"
                },
                 new Product()
                {
                    Id = "602d214e773f2a3990b47ff8",
                    Name = "Xiaomi Mi 9",
                    Description = "Descrição do Xiaomi",
                    Image = "product-4.png",
                    Price = 749.99m,
                    Category = "Smartphone"
                },
                 new Product()
                {
                    Id = "602d214e773f2a3990b47ff9",
                    Name = "LG G7",
                    Description = "Descrição do LG",
                    Image = "product-5.png",
                    Price = 799.99m,
                    Category = "Smartphone"
                }
            };
        }
    }
}