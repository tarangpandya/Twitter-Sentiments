using Serilog;
using Tweet_Publisher_Api.Utilities;

namespace Tweet_Publisher_Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(builder.Configuration)
                        .WriteTo.Console()
                        .Enrich.FromLogContext()
                        .CreateLogger();

            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(logger);


            // Add services to the container.
            try
            {
                Serilog.Debugging.SelfLog.Enable(Console.Error);
                Log.Information("Starting web host");
                builder.Services.AddControllers();


                builder.Services.CofigureApplicationServices(builder.Configuration);

                builder.Services.ConfigureTwitterHttpClient(builder.Configuration, logger);

                var app = builder.Build();

                // Configure the HTTP request pipeline.

                app.UseAuthorization();


                app.MapControllers();

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}