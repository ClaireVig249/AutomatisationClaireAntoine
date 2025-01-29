using int_db.scripts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Ajouter le support des contrôleurs API
builder.Services.AddControllers();

// Ajouter les services Database et MinIO pour injection de dépendances
builder.Services.AddSingleton<Database>(sp => new Database
{
    Host = "db",
    Name = "calc",
    User = "root",
    Password = "password"
});

builder.Services.AddSingleton<MinioClientWrapper>(sp => new MinioClientWrapper
{
    Host = "minio",
    AccessKey = "minioadmin",
    SecretKey = "minioadmin"
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapControllers(); // Permet aux API d'être accessibles

app.Run();