using EldenRing.Api.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Adiciono suporte a controladores e configuro o JSON para ignorar ciclos de referência entre objetos
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Evita erro "A possible object cycle was detected"
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });


// Configuro o Entity Framework para utilizar SQLite com a string de conexão definida
builder.Services.AddDbContext<EldenRingContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("EldenRingSqlite") ?? "Data Source=eldenring.db")
);

// Adiciono serviços do Swagger para documentação e teste da API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuro a política de CORS para permitir requisições de qualquer origem
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", p =>
    {
        p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

// Crio um escopo temporário para inicializar o banco de dados e aplicar os dados de seed
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<EldenRingContext>();
    db.Database.EnsureCreated(); // cria o banco e aplica HasData
}

// Habilito a interface do Swagger e redirecionamentos
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

// Inicio a execução da aplicação web
app.Run();
