using _3dShop.Api.Data;
using _3dShop.Api.Helpers;
using _3dShop.Api.Middlewares;
using _3dShop.Api.Services;
using _3dShop.Api.Services.Internals;
using _3dShop.Api.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Serilog;
using System.Text;

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

// Configuranso services no container
builder.Services.AddSingleton<JwtHelper>(); //Registra o jwthelper no DI. Os secrets recebidos e usados no jwthelper são automaticamente recuperados ao criar o builder em builder.Configuration
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer(); //Swagger
builder.Services.AddHttpContextAccessor(); //Permite utilizar o httpContext em outras classes além do controller (no meu caso, no jwthelper)


//Validators
builder.Services.AddValidatorsFromAssemblyContaining<CategoryValidator>(); //Com o pacote fluentvalidation.dependencyinjectionextensions, apenas essa linha busca todos os validators que herdam AbstractValidator

//Services
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<CreateUserService>();
builder.Services.AddScoped<CategoryService>();

//Configuração do swagger para rodar com JWT
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme."
    });
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("bearer", document)] = []
    });
});

// Configurando o UseAuthentication para usar o JwtBearer na validação e autenticação do token enviado na requisição
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    var jwt = builder.Configuration.GetSection("JwtSettings");

    options.TokenValidationParameters = new TokenValidationParameters
    { 
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ClockSkew = TimeSpan.Zero, // 🔥 remove tolerância de expiração

        ValidIssuer = jwt["Issuer"],
        ValidAudience = jwt["Audience"],

        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwt["Secret"]))
    };

    options.Events = new JwtBearerEvents
    {
        OnChallenge = async context =>
        {
            context.HandleResponse();
             
              context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(new
            {
                message = "Você precisa estar autenticado para acessar este recurso."
            });
        },

        OnForbidden = async context =>
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(new
            {
                message = "Você não possui permissão para acessar este recurso."
            });
        }
    };
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger(); //Swagger
    app.UseSwaggerUI(); //Swagger
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication(); //Middleware que verifica autenticidade do JWT (QUEM É VOCÊ?)
app.UseAuthorization(); //Aplica regras do Data Annotation [Authorize] (O QUE VOCÊ PODE FAZER)
app.MapControllers();
app.Run();