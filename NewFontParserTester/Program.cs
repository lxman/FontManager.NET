using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace NewFontParserTester
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (File.Exists(@"C:\tmp\FontParser.log")) File.Delete(@"C:\tmp\FontParser.log");

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(@"C:\tmp\FontParser.log")
                .CreateLogger();

            ServiceProvider services = CreateServices();
            var tester = services.GetRequiredService<Tester>();
            tester.Run();
        }

        private static ServiceProvider CreateServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton(new Tester());
            return services.BuildServiceProvider();
        }
    }
}
