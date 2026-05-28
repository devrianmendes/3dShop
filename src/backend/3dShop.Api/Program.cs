using _3dShop.Api.Data;
using _3dShop.Api.Helpers;
using _3dShop.Api.Middlewares;
using _3dShop.Api.Models.DTOs;
using _3dShop.Api.Services;
using _3dShop.Api.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// String de conexão com o banco de dados
var conString = builder.Configuration.GetConnectionString("LocalConnection")
    ?? throw new InvalidOperationException("DefaultConnection string not found.");

// Substitui o logger padrão pelo Serilog
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console());

// Add services to the container.
builder.Services.AddSingleton<JwtHelper>(); //Registra o jwthelper no DI. Os secrets recebidos e usados no jwthelper são automaticamente recuperados ao criar o builder em builder.Configuration
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer(); //Swagger

//Validators
builder.Services.AddScoped<IValidator<AuthUserRequest>, UserValidator>();

//Services
builder.Services.AddScoped<AuthService>();
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

//Context
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(conString)); //Context

//Teste de conexão com o banco de dados
//var options = new DbContextOptionsBuilder<AppDbContext>()
//    .UseNpgsql(conString)
//    .Options;
//try
//{
//    using var dbInstance = new AppDbContext(options);
//    bool conTest = dbInstance.Database.CanConnect();

//    if (conTest)
//    {
//        Console.WriteLine("Connection success.");

//    }
//    else
//    {
//        Console.WriteLine("Connection failure.");
//    }
//}
//catch (Exception ex)
//{
//    Console.WriteLine("Erro ao conectar: " + ex.Message);
//}

builder.Services.AddScoped<SeedData>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger(); //Swagger
    app.UseSwaggerUI(); //Swagger
}

app.UseHttpsRedirection();

app.UseAuthorization(); //Verifica permissões (roles)

app.UseAuthentication();

app.MapControllers();

app.Run();