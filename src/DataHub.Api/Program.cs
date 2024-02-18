using DataHub.Api.Configuration;
using DataHub.Server.Apis;

namespace DataHub.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddSwaggerGenConfig();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddCustomServices(builder.Configuration);
        builder.Services.AddDistributedMemoryCache();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwaggerExtended();
        }

        app.UseHttpsRedirection();
        app.MapSwagger();
        app.MapDataAggregation();

        app.Run();
    }
}