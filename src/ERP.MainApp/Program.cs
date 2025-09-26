using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ERP.Core.Data;
using ERP.Core.Services;
using ERP.MainApp.UI;

namespace ERP.MainApp;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("=== ChurrasTech ERP - Ambiente Produção ===");
        Console.WriteLine("Sistema de Gestão para Churrascaria");
        Console.WriteLine();

        // Configure services
        var host = CreateHostBuilder(args).Build();

        // Initialize database
        await InitializeDatabase(host);

        // Start main menu
        var mainMenu = host.Services.GetRequiredService<MainMenu>();
        await mainMenu.ShowAsync();
    }

    static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                // Configure Entity Framework (using in-memory for demo)
                services.AddDbContext<ChurrasDbContext>(options =>
                    options.UseInMemoryDatabase("ChurrasProduction"));

                // Register services
                services.AddScoped<IProdutoService, ProdutoService>();
                
                // Register UI components
                services.AddScoped<MainMenu>();
                services.AddScoped<ProdutoMenu>();
            });

    static string GetConnectionString(string environment)
    {
        // In a real application, this would come from configuration
        return environment switch
        {
            "Production" => "server=localhost;database=churrastech_prod;uid=root;pwd=;",
            "Test" => "server=localhost;database=churrastech_test;uid=root;pwd=;",
            _ => throw new ArgumentException($"Unknown environment: {environment}")
        };
    }

    static async Task InitializeDatabase(IHost host)
    {
        using var scope = host.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ChurrasDbContext>();
        
        try
        {
            await context.Database.EnsureCreatedAsync();
            Console.WriteLine("✓ Banco de dados inicializado com sucesso");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠ Erro ao inicializar banco de dados: {ex.Message}");
            Console.WriteLine("Pressione qualquer tecla para continuar sem banco...");
            Console.ReadKey();
        }
    }
}
