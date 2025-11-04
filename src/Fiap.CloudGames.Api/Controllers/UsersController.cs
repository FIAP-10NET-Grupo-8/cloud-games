using Fiap.CloudGames.Application.Common;
using Fiap.CloudGames.Application.Users.Dtos;
using Fiap.CloudGames.Application.Users.Services;
using Fiap.CloudGames.Domain.Users.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Security.Claims;

namespace Fiap.CloudGames.Api.Controllers;

/// <summary>
/// Endpoints para gerenciamento de usuários.
/// </summary>
/// <param name="userService">Serviço de usuários utilizado pelos endpoints.</param>
[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class UsersController(IUserService userService) : ControllerBase
{
	private readonly IUserService _userService = userService;

	/// <summary>
	/// Autentica usuário e retorna um JWT.
	/// </summary>
	/// <param name="dto">DTO com email e senha.</param>
	/// <param name="ct"></param>
	/// <returns>JWT token.</returns>
	[HttpPost("login")]
	[AllowAnonymous]
	[ProducesResponseType(typeof(LoginResultDto), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> Login([FromBody] LoginDto dto, CancellationToken ct)
	{
		var result = await _userService.AuthenticateAsync(dto.Email, dto.Password, ct);
		if (result == null) return Unauthorized(BasicResult.Unauthorized("Credenciais inválidas."));
		return Ok(result);
	}

	/// <summary>
	/// Lista todos os usuários (necessário role Administrator).
	/// <param name="ct"></param>
	/// </summary>
	[HttpGet]
	[Authorize(Roles = nameof(UserRole.Administrator))]
	[ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	public async Task<IActionResult> GetAll(CancellationToken ct)
	{
		var all = await _userService.GetAllAsync(ct);
		return Ok(all);
	}

	/// <summary>
	/// Obtém um usuário pelo identificador (necessário role Administrator).
	/// </summary>
	/// <param name="id">Identificador do usuário (GUID).</param>
	/// <param name="ct"></param>
	[HttpGet("{id:guid}")]
	[Authorize(Roles = nameof(UserRole.Administrator))]
	[ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
	{
		var user = await _userService.GetByIdAsync(id, ct);
		if (user == null) return NotFound(BasicResult.NotFound("Usuário não encontrado."));
		return Ok(user);
	}

	/// <summary>
	/// Obtém os dados do usuário autenticado (a partir do token JWT).
	/// </summary>
	/// <param name="ct"></param>
	[HttpGet("me")]
	[Authorize]
	[ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetMe(CancellationToken ct)
	{
		var email = User.FindFirstValue(ClaimTypes.Email);
		if (string.IsNullOrWhiteSpace(email)) return Unauthorized(BasicResult.Unauthorized("Email não presente no token."));

		var user = await _userService.GetByEmailAsync(email, ct);
		if (user == null) return Unauthorized(BasicResult.Unauthorized("Usuário não encontrado ou token inválido."));
		return Ok(user);
	}

	/// <summary>
	/// Registra um novo usuário (self-signup).
	/// </summary>
	/// <param name="dto">Dados de registro (nome, email, senha).</param>
	/// <param name="ct"></param>
	/// <returns>Usuário criado.</returns>
	[HttpPost("register")]
	[AllowAnonymous]
	[ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> Register([FromBody] UserRegisterDto dto, CancellationToken ct)
	{
		var created = await _userService.RegisterAsync(dto, ct);
		return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
	}

	/// <summary>
	/// Confirma o email de um usuário com token.
	/// </summary>
	/// <param name="dto">DTO contendo o token de confirmação.</param>
	/// <param name="ct"></param>
	[HttpPost("confirm")]
	[AllowAnonymous]
	[ProducesResponseType(typeof(BasicResult), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> Confirm([FromBody] ConfirmEmailDto dto, CancellationToken ct)
	{
		var ok = await _userService.ConfirmEmailAsync(dto.Token, ct);
		if (!ok) return BadRequest(BasicResult.BadRequest("Token inválido ou expirado."));
		return Ok(BasicResult.Success("Email confirmado com sucesso."));
	}

	/// <summary>
	/// Solicita um token de redefinição de senha para o email informado.
	/// </summary>
	/// <param name="dto">DTO com o email.</param>
	/// <param name="ct"></param>
	//  SECURITY: avaliar trocar para 202 + resposta genérica em produção
	[HttpPost("forgot-password")]
	[AllowAnonymous]
	[ProducesResponseType(typeof(ForgotPasswordResultDto), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto, CancellationToken ct)
	{
		var token = await _userService.GeneratePasswordResetAsync(dto.Email, ct);
		return Ok(new ForgotPasswordResultDto(token));
	}

	/// <summary>
	/// Redefine a senha utilizando token enviado por email.
	/// </summary>
	/// <param name="dto">DTO com token e nova senha.</param>
	/// <param name="ct"></param>
	[HttpPost("reset-password")]
	[AllowAnonymous]
	[ProducesResponseType(typeof(BasicResult), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> Reset([FromBody] ResetPasswordDto dto, CancellationToken ct)
	{
		var ok = await _userService.ResetPasswordAsync(dto.Token, dto.NewPassword, ct);
		if (!ok) return BadRequest(BasicResult.BadRequest("Token de redefinição inválido ou expirado."));
		return Ok(BasicResult.Success("Senha redefinida com sucesso."));
	}

	/// <summary>
	/// Cria um usuário com privilégios administrativos (necessário role Administrator).
	/// </summary>
	/// <param name="dto">DTO contendo nome, email e role.</param>
	/// <param name="ct"></param>
	[HttpPost]
	[Authorize(Roles = nameof(UserRole.Administrator))]
	[ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> CreateByAdmin([FromBody] AdminUserCreateDto dto, CancellationToken ct)
	{
		var created = await _userService.CreateByAdminAsync(dto, ct);
		return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
	}

	/// <summary>
	/// Endpoint para primeiro acesso: define a senha usando o token recebido por email.
	/// </summary>
	/// <param name="dto"></param>
	/// <param name="ct"></param>
	[HttpPost("first-access")]
	[AllowAnonymous]
	[ProducesResponseType(typeof(BasicResult), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> FirstAccess([FromBody] FirstAccessDto dto, CancellationToken ct)
	{
		var ok = await _userService.FirstAccessAsync(dto.Token, dto.NewPassword, ct);
		if (!ok) return BadRequest(BasicResult.BadRequest("Token inválido ou expirado."));
		return Ok(BasicResult.Success("Senha definida com sucesso."));
	}

	/// <summary>
	/// Atualiza um usuário (necessário role Administrator).
	/// </summary>
	/// <param name="dto">DTO contendo campos a serem atualizados.</param>
	/// <param name="ct"></param>
	[HttpPut]
	[Authorize(Roles = nameof(UserRole.Administrator))]
	[ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Update([FromBody] AdminUserUpdateDto dto, CancellationToken ct)
	{
		var updated = await _userService.UpdateAsync(dto, ct);
		if (updated == null) return NotFound(BasicResult.NotFound("Usuário não encontrado."));
		return Ok(updated);
	}

	/// <summary>
	/// Soft-delete (marca usuário como Deleted) (necessário role Administrator).
	/// </summary>
	/// <param name="id">Identificador do usuário (GUID).</param>
	/// <param name="ct"></param>
	[HttpDelete("{id:guid}")]
	[Authorize(Roles = nameof(UserRole.Administrator))]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
	{
		await _userService.DeleteAsync(id, ct);
		return NoContent();
	}

	/// <summary>
	/// Restaura um usuário deletado (necessário role Administrator).
	/// </summary>
	/// <param name="id"></param>
	/// <param name="ct"></param>
	[HttpPost("{id:guid}/restore")]
	[Authorize(Roles = nameof(UserRole.Administrator))]
	[ProducesResponseType(typeof(BasicResult), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	public async Task<IActionResult> Restore(Guid id, CancellationToken ct)
	{
		await _userService.RestoreAsync(id, ct);
		return Ok(BasicResult.Success("Usuário restaurado."));
	}
}
