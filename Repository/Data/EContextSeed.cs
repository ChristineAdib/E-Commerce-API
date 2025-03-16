using Core.Entities;
using Core.Entities.Order_Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Repository.Data
{
    public static class EContextSeed
    {
        public static async Task SeedAsync(EContext dbContext)
        {
            if (!dbContext.Categories.Any()) 
            {
                var CategoriesData = File.ReadAllText("../Repository/Data/DataSeed/Categories.json");
                var Categories = JsonSerializer.Deserialize<List<Category>>(CategoriesData);
                if (Categories?.Count() > 0)
                {
                    foreach (var category in Categories)
                    {
                        await dbContext.Set<Category>().AddAsync(category);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }

            if (!dbContext.Products.Any()) 
            {
                var ProductsData = File.ReadAllText("../Repository/Data/DataSeed/Products.json");
                var Products = JsonSerializer.Deserialize<List<Product>>(ProductsData);
                if (Products?.Count() > 0)
                {
                    foreach (var product in Products)
                    {
                        await dbContext.Set<Product>().AddAsync(product);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }

            if (!dbContext.DeliveryMethods.Any())
            {
                var DeliveryMethodsData = File.ReadAllText("../Repository/Data/DataSeed/delivery.json");
                var DeliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodsData);
                if (DeliveryMethods?.Count() > 0)
                {
                    foreach (var DeliveryMethod in DeliveryMethods)
                    {
                        await dbContext.Set<DeliveryMethod>().AddAsync(DeliveryMethod);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
