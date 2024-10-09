using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using glissvinyls_plus.Models;
using glissvinyls_plus.Context; // Asegúrate de incluir el contexto
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace glissvinyls_plus.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context; // Contexto de la base de datos

        public AuthController(IConfiguration configuration, AppDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        // Modelo para registro
        [HttpPost("register")]
        public IActionResult Register([FromBody] User newUser)
        {
            // Verificar que el usuario no exista ya
            if (_context.Users.Any(u => u.Username == newUser.Username))
            {
                return BadRequest(new { message = "Username already exists" });
            }

            // Aquí podrías usar BCrypt o un hash para la contraseña antes de guardar
            newUser.Password = HashPassword(newUser.Password); // Método para hash

            // Agregar el nuevo usuario a la base de datos
            _context.Users.Add(newUser);
            _context.SaveChanges();

            return Ok(new { message = "User registered successfully" });
        }

        ////PARA USAR COOKIES
        //[HttpPost("login")]
        //public IActionResult Login([FromBody] UserLogin login)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var user = _context.Users.SingleOrDefault(u => u.Username == login.Username);
        //    if (user != null && VerifyPassword(login.Password, user.Password))
        //    {
        //        var token = GenerateJwtToken(user);

        //        var cookieOptions = new CookieOptions
        //        {
        //            HttpOnly = true,
        //            Expires = DateTime.UtcNow.AddHours(1),
        //            SameSite = SameSiteMode.Lax, // Lax es menos restrictivo que Strict
        //            Secure = !Request.Host.Host.Contains("localhost") // Solo 'Secure' en producción
        //        };



        //        Response.Cookies.Append("jwtToken", token, cookieOptions);

        //        return Ok(new { Message = "Login successful" });
        //    }
        //    return Unauthorized(new { message = "Invalid username or password" });
        //}

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLogin login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _context.Users.SingleOrDefault(u => u.Username == login.Username);
            if (user != null && VerifyPassword(login.Password, user.Password))
            {
                var token = GenerateJwtToken(user);

                // Simplemente devolvemos el token en el cuerpo de la respuesta
                return Ok(new { token });
            }

            return Unauthorized(new { message = "Invalid username or password" });
        }



        // Método para generar el token, incluye el rol del usuario
        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role) // Agregar el rol
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string enteredPassword, string storedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedPassword);
        }


        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwtToken");
            return Ok(new { message = "Logout successful" });
        }
    }
}
