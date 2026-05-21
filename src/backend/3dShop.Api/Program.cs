using _3dShop.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// String de conexão com o banco de dados
var conString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("DefaultConnection string not found.");

// Substitui o logger padrão pelo Serilog
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console());

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer(); //Swagger
builder.Services.AddSwaggerGen(options =>  //Configuração do swagger para rodar com JWT
{
    options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme."
    });
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("bearer", document)] = []
    });
});
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(conString)); //Context

//Teste de conexão com o banco de dados
var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseNpgsql(conString)
    .Options;
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger(); //Swagger
    app.UseSwaggerUI(); //Swagger
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
