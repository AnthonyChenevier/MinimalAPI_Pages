using Supabase;

namespace MinimalAPI_Pages;

internal class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        bool useSwagger = builder.Configuration.GetValue<bool>("UseSwagger");

        ConfigureServices(builder, useSwagger);

        WebApplication app = BuildApp(builder, useSwagger);

        app.Run();
    }

    private static void ConfigureServices(WebApplicationBuilder builder, bool useSwagger)
    {
        // Add services to the container.
        builder.Services.AddRazorPages();

        if (useSwagger)
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
        }

        ConfigureSupabase(builder.Services, builder.Configuration);
    }

    private static void ConfigureSupabase(IServiceCollection services, IConfiguration config)
    {
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

        app.MapStaticAssets();

        app.MapRazorPages().WithStaticAssets();

        return app;
    }
}