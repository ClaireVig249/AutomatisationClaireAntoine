using int_db.scripts;

var builder = WebApplication.CreateBuilder(args);

// Ajouter le support des contrôleurs API
builder.Services.AddControllers();

// Ajouter les services Database et MinIO pour injection de dépendances
var db = builder.Services.AddSingleton<Database>(sp => new Database 
{
    Host = "db",
    Name = "calc",
    User = "root",
    Password = "password"
});

var minio = builder.Services.AddSingleton<MinioClientWrapper>(sp => new MinioClientWrapper 
{
    Endpoint = "minio:9000",
    AccessKey = "minio",
    SecretKey = "password"
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapControllers(); // Permet aux API d'être accessibles

app.Run("http://0.0.0.0:80");