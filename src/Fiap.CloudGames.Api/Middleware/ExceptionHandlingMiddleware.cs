using FluentValidation;
using System.Net;
using System.Text.Json;

namespace Fiap.CloudGames.Api.Middleware;

/// <summary>
/// Middleware para tratamento global de exceções.
/// </summary>
public sealed class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
	private static readonly JsonSerializerOptions options = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

	private readonly RequestDelegate _next = next;
	private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;

	/// <summary>
	/// Invoca o middleware.
	/// </summary>
	/// <param name="context"></param>
	/// <returns></returns>
	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await _next(context);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Unhandled exception caught by middleware");
			await HandleExceptionAsync(context, ex);
		}
	}

	private static Task HandleExceptionAsync(HttpContext context, Exception exception)
	{
		var (statusCode, errorObj) = MapExceptionToResponse(exception);

		context.Response.ContentType = "application/json";
		context.Response.StatusCode = (int)statusCode;

		var json = JsonSerializer.Serialize(errorObj, options);

		return context.Response.WriteAsync(json);
	}

	private static (HttpStatusCode statusCode, object payload) MapExceptionToResponse(Exception exception)
	{
		switch (exception)
		{
			case ValidationException fvEx:
				{
					var errors = fvEx.Errors?.Select(e => new { e.PropertyName, e.ErrorMessage }) ?? Enumerable.Empty<object>();
					return (HttpStatusCode.BadRequest, new
					{
						status = 400,
						error = "Validação falhou",
						details = errors
					});
				}

			case ArgumentNullException _:
			case ArgumentException _:
				return (HttpStatusCode.BadRequest, new { status = 400, error = exception.Message });

			case UnauthorizedAccessException _:
				return (HttpStatusCode.Unauthorized, new { status = 401, error = "Unauthorized" });

			case KeyNotFoundException _:
				return (HttpStatusCode.NotFound, new { status = 404, error = exception.Message });

			case InvalidOperationException _:
				return (HttpStatusCode.Conflict, new { status = 409, error = exception.Message });

			case NotImplementedException _:
				return (HttpStatusCode.NotImplemented, new { status = 501, error = "Not implemented" });

			default:
				return (HttpStatusCode.InternalServerError, new
				{
					status = 500,
					error = "Ocorreu um erro interno no servidor. Tente novamente mais tarde."
				});
		}
	}
}
