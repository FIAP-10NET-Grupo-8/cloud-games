namespace Fiap.CloudGames.Api.Middleware;

/// <summary>
/// Middleware para gerenciar Correlation ID em requisições HTTP.
/// </summary>
public sealed class CorrelationIdMiddleware(RequestDelegate next)
{
	/// <summary>
	/// Nome do cabeçalho HTTP para o Correlation ID.
	/// </summary>
	public const string HeaderName = "X-Correlation-Id";
	private readonly RequestDelegate _next = next;

	/// <summary>
	/// Invoca o middleware.
	/// </summary>
	/// <param name="context"></param>
	/// <returns></returns>
	public async Task InvokeAsync(HttpContext context)
	{
		if (!context.Request.Headers.TryGetValue(HeaderName, out var correlationId) || string.IsNullOrWhiteSpace(correlationId))
		{
			correlationId = Guid.NewGuid().ToString();
		}

		context.Items[HeaderName] = correlationId.ToString();

		context.Response.OnStarting(() =>
		{
			if (!context.Response.Headers.ContainsKey(HeaderName))
			{
				context.Response.Headers[HeaderName] = correlationId.ToString();
			}

			return Task.CompletedTask;
		});

		await _next(context);
	}
}
