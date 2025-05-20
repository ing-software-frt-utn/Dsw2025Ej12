using Dsw2025Ej12.Domain;
using Dsw2025Ej12.Library;
using Dsw2025Ej12.Services;
using System.Diagnostics;

namespace Dsw2025Ej12
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Conversor de precios ARS -> USD");
            var stopwatch = Stopwatch.StartNew();
            var service = new ExchangeService();
            await service.UpdatePrices();
            stopwatch.Stop();
            Console.WriteLine($"Tiempo de ejecución: {stopwatch.ElapsedMilliseconds}");
        }
    }
}
