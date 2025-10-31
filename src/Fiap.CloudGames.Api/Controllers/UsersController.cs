using Fiap.CloudGames.Application.Users.Dtos;
using Fiap.CloudGames.Application.Users.Services;
using Fiap.CloudGames.Domain.Shared.Interfaces;
using Fiap.CloudGames.Domain.Users.Enums;
using Fiap.CloudGames.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fiap.CloudGames.Api.Controllers;

/// <summary>
/// Endpoints para gerenciamento de usuários.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService userService, IEmailService emailService, JwtService jwtService) : ControllerBase
{
	private readonly IUserService _userService = userService;
	private readonly IEmailService _emailService = emailService;
	private readonly JwtService _jwtService = jwtService;

	/// <summary>
	/// Autentica usuário e retorna um JWT.
	/// </summary>
	/// <param name="dto">DTO com email e senha.</param>
	/// <returns>JWT token.</returns>
	[HttpPost("login")]
	[AllowAnonymous]
	public async Task<IActionResult> Login([FromBody] LoginDto dto)
	{
		var user = await _userService.AuthenticateAsync(dto.Email, dto.Password);
		if (user == null) return Unauthorized(new { message = "Credenciais inválidas." });
		var jwt = _jwtService.GenerateToken(user.Id, user.Name, user.Email, user.Role.ToString());
		return Ok(new { token = jwt });
	}

	/// <summary>
	/// Lista todos os usuários (necessário role Administrator).
	/// </summary>
	[HttpGet]
	[Authorize(Roles = nameof(UserRole.Administrator))]
	public async Task<IActionResult> GetAll()
	{
		var all = await _userService.GetAllAsync();
		return Ok(all);
	}

	/// <summary>
	/// Obtém um usuário pelo identificador (necessário role Administrator).
	/// </summary>
	/// <param name="id">Identificador do usuário (GUID).</param>
	[HttpGet("{id:guid}")]
	[Authorize(Roles = nameof(UserRole.Administrator))]
	public async Task<IActionResult> GetById(Guid id)
	{
		var user = await _userService.GetByIdAsync(id);
		if (user == null) return NotFound(new { message = "Usuário não encontrado." });
		return Ok(user);
	}

	/// <summary>
	/// Obtém os dados do usuário autenticado (a partir do token JWT).
	/// </summary>
	[HttpGet("me")]
	[Authorize]
	public async Task<IActionResult> GetMe()
	{
		var email = User.FindFirstValue(ClaimTypes.Email);
		if (string.IsNullOrWhiteSpace(email)) return Unauthorized(new { message = "Email não presente no token." });

		var user = await _userService.GetByEmailAsync(email);
		if (user == null) return Unauthorized(new { message = "Usuário não encontrado ou token inválido." });
		return Ok(user);
	}

	/// <summary>
	/// Registra um novo usuário (self-signup).
	/// </summary>
	/// <param name="dto">Dados de registro (nome, email, senha).</param>
	/// <returns>Usuário criado.</returns>
	[HttpPost("register")]
	[AllowAnonymous]
	public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
	{
		var created = await _userService.RegisterAsync(dto);
		var token = await _userService.GenerateEmailConfirmationAsync(created.Email);
		await _emailService.SendEmailAsync(created.Email, "Confirmação de email", $"Seu token de confirmação: {token}");
		return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
	}

	/// <summary>
	/// Confirma o email de um usuário com token.
	/// </summary>
	/// <param name="dto">DTO contendo o token de confirmação.</param>
	[HttpPost("confirm")]
	[AllowAnonymous]
	public async Task<IActionResult> Confirm([FromBody] ConfirmEmailDto dto)
	{
		var ok = await _userService.ConfirmEmailAsync(dto.Token);
		if (!ok) return BadRequest(new { message = "Token inválido ou expirado." });
		return Ok(new { message = "Email confirmado com sucesso." });
	}

    /// <summary>
    /// Solicita um token de redefinição de senha para o email informado.
    /// </summary>
    /// <param name="dto">DTO com o email.</param>
    //  SECURITY: avaliar trocar para 202 + resposta genérica em produção
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
	{
		var token = await _userService.GeneratePasswordResetAsync(dto.Email);
		return Ok(new { resetToken = token });
	}

	/// <summary>
	/// Redefine a senha utilizando token enviado por email.
	/// </summary>
	/// <param name="dto">DTO com token e nova senha.</param>
	[HttpPost("reset-password")]
	[AllowAnonymous]
	public async Task<IActionResult> Reset([FromBody] ResetPasswordDto dto)
	{
		var ok = await _userService.ResetPasswordAsync(dto.Token, dto.NewPassword);
		if (!ok) return BadRequest(new { message = "Token de redefinição inválido ou expirado." });
		return Ok(new { message = "Senha redefinida com sucesso." });
	}

	/// <summary>
	/// Cria um usuário com privilégios administrativos (necessário role Administrator).
	/// </summary>
	/// <param name="dto">DTO contendo nome, email e role.</param>
	[HttpPost]
	[Authorize(Roles = nameof(UserRole.Administrator))]
	public async Task<IActionResult> CreateByAdmin([FromBody] AdminUserCreateDto dto)
	{
		var created = await _userService.CreateByAdminAsync(dto);
		var token = await _userService.GenerateFirstAccessAsync(created.Email);
		await _emailService.SendEmailAsync(created.Email, "Acesso inicial - defina sua senha", $"Utilize este token para definir sua senha inicial: {token}");
		return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
	}

	/// <summary>
	/// Endpoint para primeiro acesso: define a senha usando o token recebido por email.
	/// </summary>
	[HttpPost("first-access")]
	[AllowAnonymous]
	public async Task<IActionResult> FirstAccess([FromBody] FirstAccessDto dto)
	{
		var ok = await _userService.FirstAccessAsync(dto.Token, dto.NewPassword);
		if (!ok) return BadRequest(new { message = "Token inválido ou expirado." });
		return Ok(new { message = "Senha definida com sucesso." });
	}

	/// <summary>
	/// Atualiza um usuário (necessário role Administrator).
	/// </summary>
	/// <param name="dto">DTO contendo campos a serem atualizados.</param>
	[HttpPut]
	[Authorize(Roles = nameof(UserRole.Administrator))]
	public async Task<IActionResult> Update([FromBody] AdminUserUpdateDto dto)
	{
		var updated = await _userService.UpdateAsync(dto);
		if (updated == null) return NotFound(new { message = "Usuário não encontrado." });
		return Ok(updated);
	}

	/// <summary>
	/// Soft-delete (marca usuário como Deleted) (necessário role Administrator).
	/// </summary>
	/// <param name="id">Identificador do usuário (GUID).</param>
	[HttpDelete("{id:guid}")]
	[Authorize(Roles = nameof(UserRole.Administrator))]
	public async Task<IActionResult> Delete(Guid id)
	{
		await _userService.DeleteAsync(id);
		return NoContent();
	}

	/// <summary>
	/// Restaura um usuário deletado (necessário role Administrator).
	/// </summary>
	[HttpPost("{id:guid}/restore")]
	[Authorize(Roles = nameof(UserRole.Administrator))]
	public async Task<IActionResult> Restore(Guid id)
	{
		await _userService.RestoreAsync(id);
		return Ok(new { message = "Usuário restaurado." });
	}
}
