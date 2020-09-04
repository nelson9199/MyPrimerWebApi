using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration.AzureKeyVault;
namespace MyPrimerWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                // .ConfigureAppConfiguration((env, config) =>
                // {
                //     // Aqui colocamos la configuracion de proveedores
                //     var ambiente = env.HostingEnvironment.EnvironmentName;
                //     config.AddJsonFile($"appsettings.{ambiente}.json", optional: true, reloadOnChange: true);
                //     config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                //     config.AddEnvironmentVariables();
                //     if (args != null)
                //     {
                //         config.AddCommandLine(args);
                //     }
                //     // Con build configuramos los proveedores de configuracion que tenemos declarados hasta ahora
                //     var currentConfig = config.Build();
                //     config.AddAzureKeyVault(currentConfig["Vault"], currentConfig["ClientId"], currentConfig["ClientSecret"]);
                // })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
