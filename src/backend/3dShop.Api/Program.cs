using _3dShop.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.WebEncoders.Testing;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var conString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("DefaultConnection string not found.");

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(conString));

var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql(conString)
                .Options;

//using var teste = new AppDbContext(options);
try
{
    using var teste = new AppDbContext(options);
    teste.Database.OpenConnection(); // força a abertura
    Console.WriteLine("Conexão OK!");
    teste.Database.CloseConnection();
}
catch (Exception ex)
{
    Console.WriteLine("Erro ao conectar: " + ex.Message);
}

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
