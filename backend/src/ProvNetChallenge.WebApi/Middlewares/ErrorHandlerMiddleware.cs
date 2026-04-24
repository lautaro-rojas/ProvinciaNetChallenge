// ProvNetChallenge.WebApi/Middlewares/ErrorHandlerMiddleware.cs
using ProvNetChallenge.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace ProvNetChallenge.WebApi.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                // Deja que la petición siga su curso hacia el Controller y el Service
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                // Evaluamos qué tipo de excepción lanzó nuestro servicio
                switch (error)
                {
                    case BadRequestException e:
                        // Excepción controlada de negocio -> 400
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    default:
                        // Excepción no controlada (ej. se cayó SQL Server) -> 500
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var result = JsonSerializer.Serialize(new { message = error?.Message });
                await response.WriteAsync(result);
            }
        }
    }
}