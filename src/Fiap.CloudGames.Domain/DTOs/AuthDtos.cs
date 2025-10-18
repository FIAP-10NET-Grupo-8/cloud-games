using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiap.CloudGames.Domain.DTOs
{
    public class AuthDtos
    {
        public class LoginRequestDto([Required] string Username, [Required] string Password);
        public class RegisterRequestDto([Required] string Username, [Required][EmailAddress] string Email, [Required] string Password);
        public class TokenResponseDto(string Token);
    }
}
