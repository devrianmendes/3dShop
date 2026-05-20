using _3dShop.Api.Data;
using Microsoft.EntityFrameworkCore;

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
    using var dbInstance = new AppDbContext(options);
    bool conTest = dbInstance.Database.CanConnect();

    if (conTest)
    {
        Console.WriteLine("Connection success.");

    }
    else
    {
        Console.WriteLine("Connection failure.");
    }
}
catch (Exception ex)
{
    Console.WriteLine("Erro ao conectar: " + ex.Message);
}

builder.Services.AddScoped<SeedData>();

var app = builder.Build();

using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

var seed = new SeedData(context);
await seed.Initialize();




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
