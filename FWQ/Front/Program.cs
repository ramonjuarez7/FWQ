using EmbedIO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Swan.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Front
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            /*
            var url = "http://localhost:9696/";
            using (WebServer server = CreateWebServer(url))
            {
                server.RunAsync();
            }
            Console.ReadKey(true);*/

            CreateHostBuilder(args).Build().Run();
        }
        private static WebServer CreateWebServer(string url)
        {
            WebServer server = new WebServer(o => o
                  .WithUrlPrefix(url)
                  .WithMode(HttpListenerMode.EmbedIO))
               .WithLocalSessionManager()
               .WithAction("/test", HttpVerbs.Any, ctx =>
               {
                   Console.WriteLine("Request received");
                   return ctx.SendDataAsync(new { mensaje = "Hola mundo" });
               }
               );
            // Listen for state changes.
            server.StateChanged += (s, e) => $"WebServer New State - {e.NewState}".Info();
            return server;
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
