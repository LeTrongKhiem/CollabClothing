using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollabClothing.Appication.Catalog.Products;
using CollabClothing.ViewModels.Catalog.Products;
using CollabClothing.WebApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using CollabClothing.Appication.Common;

namespace CollabClothing.BackendApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();

        }
        // public static async Task Main(string[] args)
        // {
        //     ManageProductService request = new ManageProductService();
        //     DBClothingContext context = new DBClothingContext();
        //     PublicProductService p = new PublicProductService(context);
        //     List<ProductViewModel> list = await p.GetAllByCategoryId();
        //     foreach (var product in list)
        //     {
        //         Console.WriteLine(product.Id);
        //     }
        // }



        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
