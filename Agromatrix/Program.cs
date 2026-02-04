using Agromatrix.Components;
using Microsoft.EntityFrameworkCore;
using Agromatrix.Data;
using Agromatrix.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddCircuitOptions(o => { o.DetailedErrors = true; }); // <-- εδώ

// Configure DbContext with PostgreSQL (log SQL and print connection string)
var cs = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine($"Using DB connection: {cs}");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(cs)
           .LogTo(Console.WriteLine)
           .EnableSensitiveDataLogging()
);

// Register ProductService
builder.Services.AddScoped<IProductService, ProductService>();

// Register CartService
builder.Services.AddScoped<CartService>();

// Register Authentication service
builder.Services.AddScoped<AuthService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Seed a small dev user if none exist
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        if (!db.Users.Any())
        {
            var admin = new Agromatrix.Models.User
            {
                Username = "admin",
                Email = "admin@local",
                IsAdmin = true
            };

            var hasher = new Microsoft.AspNetCore.Identity.PasswordHasher<Agromatrix.Models.User>();
            admin.PasswordHash = hasher.HashPassword(admin, "admin123");

            db.Users.Add(admin);
            db.SaveChanges();

            Console.WriteLine("Seeded default user: admin / admin123 (admin)");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("User seed skipped: " + ex.Message);
    }
}

app.Run();
