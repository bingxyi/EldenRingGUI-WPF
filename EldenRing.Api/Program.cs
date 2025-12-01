using EldenRing.Api.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Evita erro "A possible object cycle was detected"
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        // Se quiser, ajustar a profundidade máxima (opcional):
        // options.JsonSerializerOptions.MaxDepth = 64;
    });


// EF Core SQLite
builder.Services.AddDbContext<EldenRingContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("EldenRingSqlite") ?? "Data Source=eldenring.db")
);

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Allow minimal CORS if needed (WPF does not need CORS, but it's harmless)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", p =>
    {
        p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

// Ensure DB created and seeded
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<EldenRingContext>();
    db.Database.EnsureCreated(); // cria o banco e aplica HasData
}

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();
