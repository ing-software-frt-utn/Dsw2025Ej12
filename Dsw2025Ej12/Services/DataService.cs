using Dsw2025Ej12.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Dsw2025Ej12.Services;

internal class DataService
{
    public static List<Product>? GetProductsFromFile()
    {
        var data = File.ReadAllText("Data\\Products.json");
        return System.Text.Json
        .JsonSerializer
            .Deserialize<List<Product>>(data);
    }

    public static async Task<List<Product>?> GetProductsFromFileAsync()
    {
        var data = await File.ReadAllBytesAsync("Data\\Products.json");
        using var stream = new MemoryStream(data);
        return await System.Text.Json
        .JsonSerializer
            .DeserializeAsync<List<Product>>(stream);
    }
}
