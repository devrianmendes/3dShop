using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace YourProject.Extensions;

// Classes de extensão precisam ser estáticas (static) para injetar métodos em classes existentes
public static class AuthenticationExtensions
{
    // O modificador 'this' no primeiro parâmetro transforma este método em uma extensão do IServiceCollection
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        // Acessa a seção "JwtSettings" do arquivo appsettings.json
        var jwtSettings = configuration.GetSection("JwtSettings");

        // Extrai a string correspondente à chave secreta do JWT
        var secretKey = jwtSettings["Secret"];

        // Validação de segurança: impede que a API suba se a chave secreta esquecida ou vazia
        if (string.IsNullOrEmpty(secretKey))
        {
            // Lança uma exceção fatal interrompendo a inicialização da aplicação
            throw new InvalidOperationException("A chave secreta do JWT (JwtSettings:Secret) não foi configurada corretamente.");
        }

        // Converte a string da chave secreta em um array de bytes (padrão exigido pela biblioteca de criptografia)
        var key = Encoding.UTF8.GetBytes(secretKey);

        // Registra os serviços de autenticação do ASP.NET Core no container de Injeção de Dependência
        services.AddAuthentication(options =>
        {
            // Define que o esquema padrão de validação de identidade será via Token Bearer (JWT)
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

            // Define que o desafio padrão (quando o usuário tenta acessar sem token) também será tratado pelo JwtBearer
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        // Adiciona e configura especificamente o comportamento do middleware JwtBearer
        .AddJwtBearer(options =>
        {
            // Define se a API exige HTTPS para processar o token (false apenas para facilitar testes locais)
            options.RequireHttpsMetadata = false;

            // Determina que o token decodificado será salvo no contexto da requisição (HttpContext.User)
            options.SaveToken = true;

            // Instancia o objeto que dita as regras matemáticas de validação do token recebido
            options.TokenValidationParameters = new TokenValidationParameters
            {
                // Obriga o middleware a verificar se o emissor (Issuer) do token é o esperado
                ValidateIssuer = true,

                // Obriga o middleware a verificar se o público-alvo (Audience) do token é a sua API
                ValidateAudience = true,

                // Obriga o middleware a validar se a data atual está dentro do período de expiração do token
                ValidateLifetime = true,

                // Obriga o middleware a validar se a assinatura digital do token foi feita com a sua chave secreta
                ValidateIssuerSigningKey = true,

                // Seta o tempo de tolerância para zero (por padrão o .NET dá 5 minutos de lambuja para relógios dessincronizados)
                ClockSkew = TimeSpan.Zero,

                // Informa qual é o nome do emissor oficial que deve estar escrito dentro do token
                ValidIssuer = jwtSettings["Issuer"],

                // Informa qual é o nome do público-alvo oficial que deve estar escrito dentro do token
                ValidAudience = jwtSettings["Audience"],

                // Fornece a chave criptográfica simétrica criada a partir do seu segredo para validar a assinatura
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };

            // Abre o bloco de interceptação de eventos do ciclo de vida da autenticação
            options.Events = new JwtBearerEvents
            {
                // Este evento é disparado automaticamente quando uma requisição falha nos testes de autenticação
                OnChallenge = async context =>
                {
                    // Cancela o processamento padrão do .NET (que tentaria redirecionar para uma página de login ou retornar vazio)
                    context.HandleResponse();

                    // Altera o status HTTP da resposta para 401 (Não Autorizado)
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                    // Altera o cabeçalho Content-Type para avisar o cliente de que o retorno será um JSON
                    context.Response.ContentType = "application/json";

                    // Define variáveis locais com textos padrão (caso o usuário não envie token nenhum)
                    var title = "Não Autorizado";
                    var detail = "Você precisa fornecer um token válido para acessar este recurso.";
                    var errorCode = "AUTH_UNAUTHORIZED";

                    // Verifica se o middleware capturou alguma exceção interna de validação de token
                    if (context.AuthenticateFailure != null)
                    {
                        // Usa Pattern Matching com expressão switch para extrair mensagens com base no tipo da exceção
                        (title, detail, errorCode) = context.AuthenticateFailure switch
                        {
                            // Caso a exceção seja do tipo "Token Expirado"
                            SecurityTokenExpiredException =>
                                ("Token Expirado", "O token fornecido expirou. Por favor, solicite um novo token ou faça login novamente.", "TOKEN_EXPIRED"),

                            // Caso a exceção indique que a assinatura do token foi alterada ou é inválida
                            SecurityTokenInvalidSignatureException =>
                                ("Assinatura Inválida", "A assinatura do token não confere. O token pode ter sido adulterado.", "TOKEN_INVALID_SIGNATURE"),

                            // Caso a exceção indique que a estrutura do token está corrompida (não tem 3 partes separadas por pontos)
                            SecurityTokenMalformedException =>
                                ("Formato Inválido", "O formato do token JWT enviado está incorreto ou corrompido.", "TOKEN_MALFORMED"),

                            // Caso caia em qualquer outro erro genérico de validação de token do .NET
                            _ =>
                                ("Falha na Validação", "O token enviado falhou nos critérios de validação de segurança.", "TOKEN_VALIDATION_FAILED")
                        }; // Fim do switch expression
                    }
                    // Caso não haja uma exceção interna, mas o cabeçalho HTTP trouxe um texto descritivo nativo
                    else if (!string.IsNullOrEmpty(context.ErrorDescription))
                    {
                        // Substitui o detalhe padrão pela descrição fornecida pelo protocolo HTTP
                        detail = context.ErrorDescription;
                    }

                    // Monta o objeto anônimo seguindo rigidamente o contrato internacional RFC 7807
                    var problemDetails = new
                    {
                        // URL única que documenta o tipo do erro ocorrido na sua aplicação
                        type = $"https://suaapi.com{errorCode.ToLower()}",
                        // Resumo curto legível por humanos sobre o erro
                        title = title,
                        // Cópia do status HTTP da resposta para consistência do JSON
                        status = StatusCodes.Status401Unauthorized,
                        // Explicação detalhada explicando o motivo exato do erro ocorrer
                        detail = detail,
                        // O caminho da URL (endpoint) que o usuário tentou acessar no momento da falha
                        instance = context.Request.Path.Value,
                        // Dicionário extensível para adicionar chaves customizadas fora do padrão básico da RFC
                        extensions = new Dictionary<string, object?>
                        {
                        // Código textual de erro para facilitar a validação estrita no front-end
                        { "code", errorCode },
                        // Verifica se o ambiente atual de execução da API é "Development"
                        { "debugMessage", context.HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>().IsDevelopment()
                                ? context.AuthenticateFailure?.Message // Se for desenvolvimento, injeta a Exception nativa do C#
                                : null } // Se for produção, injeta nulo para ocultar detalhes internos do servidor
                        }
                    };

                    // Serializa o objeto criado diretamente no corpo (body) da resposta HTTP HTTP
                    await context.Response.WriteAsJsonAsync(problemDetails);
                },

                // Este evento é disparado se o usuário está autenticado, mas o endpoint exige uma Role/Policy que ele não possui
                OnForbidden = async context =>
                {
                    // Altera o status HTTP da resposta para 403 (Proibido/Acesso Negado)
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;

                    // Altera o cabeçalho Content-Type para informar o formato JSON
                    context.Response.ContentType = "application/json";

                    // Monta o JSON padrão RFC 7807 para falhas de autorização de escopo

                    var problemDetails = new
                    {
                        type = "suaapi.com",
                        title = "Acesso Proibido",
                        status = StatusCodes.Status403Forbidden,
                        detail = "Seu usuário está autenticado, mas não possui as permissões necessárias para acessar este endpoint.",
                        instance = context.Request.Path.Value,
                        extensions = new Dictionary<string, object> { { "code", "ACCESS_DENIED" } }
                    };
                    // Escreve o JSON de erro proibido na resposta HTTP final
                    await context.Response.WriteAsJsonAsync(problemDetails);
                }
            };// Fim da atribuição de eventos
        }); // Fim da configuração do AddJwtBearer
        return services;
    }
}