using Serilog;
using TweetHashtagCountSubscriberAPI.Utilities;

namespace TweetHashtagCountSubscriberAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(builder.Configuration)
                        .WriteTo.Console()
                        .Enrich.FromLogContext()
                        .CreateLogger();

            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(logger);

            try
            {
                Serilog.Debugging.SelfLog.Enable(Console.Error);
                Log.Information("Starting web host");

                builder.Services.AddControllers();
                builder.Services.CofigureApplicationServices(builder.Configuration);

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                app.UseHttpsRedirection();
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