namespace Fiap.CloudGames.Api.Middleware;

/// <summary>
/// Middleware para logging estruturado de requisições HTTP.
/// </summary>
public sealed class StructuredLoggingMiddleware(RequestDelegate next, ILogger<StructuredLoggingMiddleware> logger)
{
	private readonly RequestDelegate _next = next;
	private readonly ILogger<StructuredLoggingMiddleware> _logger = logger;
	private const string CorrelationHeader = CorrelationIdMiddleware.HeaderName;

	/// <summary>
	/// Invoca o middleware.
	/// </summary>
	/// <param name="context"></param>
	/// <returns></returns>
	public async Task InvokeAsync(HttpContext context)
	{
		var correlationId = GetCorrelationId(context);
		using (_logger.BeginScope(new Dictionary<string, object> { ["CorrelationId"] = correlationId }))
		{
			var start = DateTime.UtcNow;
			_logger.LogInformation("Handling request {Method} {Path}", context.Request.Method, context.Request.Path);

			try
			{
				await _next(context);
				var duration = DateTime.UtcNow - start;
				_logger.LogInformation("Handled request {Method} {Path} responded {StatusCode} in {Duration}ms", context.Request.Method, context.Request.Path, context.Response.StatusCode, duration.TotalMilliseconds);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Request {Method} {Path} failed", context.Request.Method, context.Request.Path);
				throw;
			}
		}
	}

	private static string GetCorrelationId(HttpContext context)
	{
		if (context.Items.TryGetValue(CorrelationHeader, out var value) && value is string s && !string.IsNullOrWhiteSpace(s))
			return s;

		if (context.Request.Headers.TryGetValue(CorrelationHeader, out var header) && !string.IsNullOrWhiteSpace(header))
			return header.ToString();

		return Guid.NewGuid().ToString();
	}
}
