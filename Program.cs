using Newtonsoft.Json;
using Supabase;

namespace MinimalAPI_Pages;

internal class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        bool useSwagger = builder.Configuration.GetValue<bool>("UseSwagger");

        ConfigureServices(builder, useSwagger);

        WebApplication app = BuildApp(builder, useSwagger);

        app.Run();
    }

    private static void ConfigureServices(WebApplicationBuilder builder, bool useSwagger)
    {
        // Add services to the container.
        builder.Services.AddRazorPages();
        builder.Services.AddControllers();
        builder.Services.AddControllers().AddNewtonsoftJson(options => { options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; });
        ConfigureSupabase(builder.Services, builder.Configuration);

        if (!useSwagger)
            return;

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
    }

    private static void ConfigureSupabase(IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<Controllers.ItemsController>();
        services.AddScoped(_ =>
        {
            string? url = config.GetValue<string>("Supabase:Url");
            string? key = config.GetValue<string>("Supabase:Key");

            if (string.IsNullOrWhiteSpace(url) || string.IsNullOrWhiteSpace(key))
                throw new Exception("Missing Supabase configuration in appsettings.json.");

            SupabaseOptions options = new SupabaseOptions
            {
                AutoRefreshToken = true,
                AutoConnectRealtime = true
            };

            return new Client(url, key, options);
        });
    }

    private static WebApplication BuildApp(WebApplicationBuilder builder, bool useSwagger)
    {
        WebApplication app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        if (useSwagger)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllers();

        app.MapStaticAssets();

        app.MapRazorPages().WithStaticAssets();

        return app;
    }
}