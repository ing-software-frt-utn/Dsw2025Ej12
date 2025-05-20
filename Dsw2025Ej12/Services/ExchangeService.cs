using Dsw2025Ej12.Domain;
using Dsw2025Ej12.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Ej12.Services;

internal class ExchangeService
{
    private async Task<decimal> GetAverageDollarQuote()
    {
        var quoteTask1 = QuoteManager.GetDollarQuoteOptionOneAsync();
        var quoteTask2 = QuoteManager.GetDollarQuoteOptionTwoAsync();
        var quoteTask3 = QuoteManager.GetDollarQuoteOptionThreeAsync();

        await Task.WhenAll(quoteTask1, quoteTask2, quoteTask3);
        var list = new[] { await quoteTask1, await quoteTask2, await quoteTask3 };
        return list.Average();
    }

    private async Task<List<Product>> GetProducts()
    {
        var products = new List<Product>();
        var task1 = DataManager.GetProductsAsync();
        var task2 = DataService.GetProductsFromFileAsync();
        var productTasks = new List<Task> { task1, task2 };
        while (productTasks.Count > 0)
        {
            Task finished = await Task.WhenAny(productTasks);
            if(finished == task1)
            {
                var result1 = await task1;
                products.AddRange(result1?
                        .Select(p => new Product
                        {
                            Code = p.Code,
                            Description = p.Description,
                            Price = p.Price
                        }) ?? []);
            }
            else if(finished == task2)
            {
                products.AddRange(await task2 ?? []);
            }
            productTasks.Remove(finished);
        }
        return products;
    }

    public async Task UpdatePrices()
    {
        var quoteTask = GetAverageDollarQuote();
        var productsTask = GetProducts();
        await Task.WhenAll(quoteTask, productsTask);  
        var quote = await quoteTask;
        var products = await productsTask;  
        products.ForEach(p => p.UpdatePrice(quote));
    }
}
