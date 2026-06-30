using _3dShop.Api.Errors;
using _3dShop.Api.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace _3dShop.Api.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger) //Recebe o próximo middleware para permitir a partida para o proximo passo do pipeline
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context) //Método responsável por capturar exceções lançadas durante a requisição
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception) //Método que mapeia as exceções recebidas para status code corretos
        {
            context.Response.ContentType = "application/json";

            context.Response.StatusCode = exception switch //Altera o statusCode da requisição a partir das exceções lançadas na aplicação. Nada tem a ver com o objeto de retorno.
            {
                NotFoundException => StatusCodes.Status404NotFound,
                BadRequestException or ValidationException => StatusCodes.Status400BadRequest,
                BusinessException => StatusCodes.Status422UnprocessableEntity,
                UnauthorizedException => StatusCodes.Status401Unauthorized,
                _ => StatusCodes.Status500InternalServerError
            };

            if (exception is ValidationException validationException) //Verifica se a exceção foi lançada pelo FluentValidation
            {
                ValidationErrorResponse errorList = new() //Objeto criado para comportar todas os possiveis erros do FluentValidation
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "Erro de validação",
                    Errors = validationException.Errors.Select(e => new ValidationError() //Objeto criado para comportar cada possível erro do FluentValidation
                    {
                        Field = e.PropertyName,
                        Message = e.ErrorMessage
                    }).ToList()
                };
                return context.Response.WriteAsJsonAsync(errorList);
            } else {
                //var response = new ExceptionErrorResponse
                //{
                //    StatusCode = context.Response.StatusCode,
                //    Message = context.Response.StatusCode == 500
                //        ? "Erro interno no servidor"
                //        : exception.Message
                //};

                //_logger.LogError(exception, exception.Message,
                //    context.Request.Method,
                //    context.Request.Path);

                //return context.Response.WriteAsJsonAsync(response);
                //var generic = new
                //{
                //    StatusCode = context.Response.StatusCode,
                //    Message = exception.Message,
                //    StackTrace = exception.StackTrace,
                //    idk = exception.InnerException
                //};

                var cont = context;
                var ex = exception;

                var problemDetails = new ProblemDetails
                {
                    Type = "",
                    Title = exception.Message,
                    Status = context.Response.StatusCode,
                    Detail = "Seu usuário está autenticado, mas não possui as permissões necessárias para acessar este endpoint.",
                    Instance = context.Request.Path.Value,
                    //Extensions = new Dictionary<string, object?> { { "code", "ACCESS_DENIED" } }
                };

                return context.Response.WriteAsJsonAsync(problemDetails);
            }

        } 
    }
}
