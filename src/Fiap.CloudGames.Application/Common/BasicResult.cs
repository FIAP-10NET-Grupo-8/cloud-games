namespace Fiap.CloudGames.Application.Common;

/// <summary>
/// Resultado básico padrão.
/// </summary>
/// <param name="StatusCode">Código de status HTTP (opcional).</param>
/// <param name="Message">Mensagem adicional (opcional).</param>
public record BasicResult(
    int? StatusCode = null,
    string? Message = null
)
{
	public static BasicResult Success(string? message = null) => new(200, message);
	public static BasicResult Accepted(string? message = null) => new(202, message);
	public static BasicResult NoContent(string? message = null) => new(204, message);
    public static BasicResult BadRequest(string? message = null) => new(400, message);
    public static BasicResult Unauthorized(string? message = null) => new(401, message);
    public static BasicResult Forbidden(string? message = null) => new(403, message);
    public static BasicResult NotFound(string? message = null) => new(404, message);
}
