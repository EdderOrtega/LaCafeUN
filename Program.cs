using Microsoft.EntityFrameworkCore;
using ProyectoFinalPOO2.Data;
using ProyectoFinalPOO2.Services;

var builder = WebApplication.CreateBuilder(args);

// Configurar puerto dinámico para Render
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// Agregar servicios
builder.Services.AddControllersWithViews();

// Configurar API Controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// Configurar Swagger para API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { 
        Title = "La Cafe API", 
        Version = "v1",
        Description = "API REST para La Cafe - Sistema de cafetería"
    });
});

// Configurar sesiones
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Configurar PostgreSQL
// En producción, las variables de entorno tienen prioridad sobre appsettings.json
var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
    ?? builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Host=localhost;Database=lacafe_db;Username=postgres;Password=postgres";

Console.WriteLine($"[INFO] Entorno: {builder.Environment.EnvironmentName}");
Console.WriteLine($"[INFO] Usando connection string: {connectionString.Substring(0, Math.Min(50, connectionString.Length))}...");

builder.Services.AddDbContext<CafeteriaContext>(options =>
    options.UseNpgsql(connectionString));

// Configurar servicio de imágenes
// En desarrollo usa LocalImagenService, en producción CloudinaryImagenService
if (builder.Environment.IsProduction())
{
    builder.Services.AddScoped<IImagenService, CloudinaryImagenService>();
}
else
{
    builder.Services.AddScoped<IImagenService, LocalImagenService>();
}

// Configurar CORS para la app MAUI
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMAUI", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Aplicar migraciones automáticamente AL INICIO
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<CafeteriaContext>();
        var logger = services.GetRequiredService<ILogger<Program>>();
        
        logger.LogInformation("=== INICIO DE MIGRACIONES ===");
        Console.WriteLine("[INFO] Verificando estado de la base de datos...");
        
        // Verificar si hay migraciones pendientes
        var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
        var appliedMigrations = await context.Database.GetAppliedMigrationsAsync();
        
        Console.WriteLine($"[INFO] Migraciones aplicadas: {appliedMigrations.Count()}");
        Console.WriteLine($"[INFO] Migraciones pendientes: {pendingMigrations.Count()}");
        
        if (pendingMigrations.Any())
        {
            Console.WriteLine("[INFO] Aplicando migraciones pendientes...");
            foreach (var migration in pendingMigrations)
            {
                Console.WriteLine($"[INFO] - {migration}");
            }
            
            await context.Database.MigrateAsync();
            Console.WriteLine("[SUCCESS] ✅ Migraciones aplicadas exitosamente!");
        }
        else
        {
            Console.WriteLine("[INFO] No hay migraciones pendientes. Base de datos actualizada.");
        }
        
        // Verificar que las tablas existan
        var canConnect = await context.Database.CanConnectAsync();
        Console.WriteLine($"[INFO] Conexión a BD: {(canConnect ? "✅ OK" : "❌ FALLO")}");
        
        logger.LogInformation("=== FIN DE MIGRACIONES ===");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "❌ ERROR CRÍTICO al aplicar migraciones de base de datos.");
        Console.WriteLine($"[ERROR] {ex.Message}");
        Console.WriteLine($"[ERROR] Stack: {ex.StackTrace}");
        
        // En producción, lanzar excepción para que Render vea el error
        throw;
    }
}

// Configurar la aplicación - SWAGGER SIEMPRE HABILITADO
app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "La Cafe API v1");
    c.RoutePrefix = "api/docs";
    c.DocumentTitle = "La Cafe API - Documentación";
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Habilitar CORS
app.UseCors("AllowMAUI");

// Habilitar sesiones
app.UseSession();
app.UseAuthorization();

// Mapear rutas MVC (para la web)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Mapear rutas API
app.MapControllers();

Console.WriteLine("[INFO] 🚀 Aplicación iniciada correctamente");
app.Run();
