using ATMSimulator.Logic;
using Microsoft.AspNetCore.Hosting;

namespace ATMSimulator // Note: actual namespace depends on the project name.
{
    public class Program
    {
        static void Main(string[] args)
        {
            //var atm = new Atm();
            //Atm.Run(atm);
            CreateHostBuilder(args).Build().Run();
        }
        public static IHostBuilder CreateHostBuilder(string[] argd) =>
           Host.CreateDefaultBuilder(argd)
           .ConfigureWebHostDefaults(webBuilder =>
           {
               webBuilder.UseStartup<StartUp>();
           });
    }
}