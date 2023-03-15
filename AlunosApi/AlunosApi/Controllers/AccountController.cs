using AlunosApi.Services;
using AlunosApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace AlunosApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthenticate _authenticate;
        public AccountController(IConfiguration configuration, IAuthenticate authenticate)
        {
            _configuration = configuration ?? throw new ArgumentException(nameof(configuration));
            _authenticate = authenticate ?? throw new ArgumentException(nameof(authenticate));

        }
        [HttpPost("CreateUser")]
        public async Task<ActionResult<UserToken>> CreateUser([FromBody] RegisterViewModel model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "As senhas não conferem");
                return BadRequest(ModelState);
            }

            var result = await _authenticate.RegisterUser(model.Email, model.Password);

            if (result)
            {
                return Ok($"Usuário {model.Email} criado com sucesso");
            }
            else
            {
                ModelState.AddModelError("CreateUser", "Registro inválido.");
                return BadRequest(ModelState);
            }
        }

        [HttpPost("LoginUser")]

        public async Task<ActionResult<UserToken>> Login([FromBody] LoginViewModel userInfo)
        {
            var result = await _authenticate.Authenticate(userInfo.Email, userInfo.Password);

            if (result)
            {
                return GenerateToken(userInfo);
            }
            else
            {
                ModelState.AddModelError("LoginUser", "Login inválido.");
                return BadRequest(ModelState);
            }
        }

        private ActionResult<UserToken> GenerateToken(LoginViewModel userInfo)
        {
            var claims = new[]
            {
                new Claim("email", userInfo.Email), //define o email
                new Claim("meuToken", "token do roberto"), //define o valor de uma classe qualquer so de exemplo que pode colocar qualquer valor
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) //aqui gera um claim especifica 
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"])); //chave secreta

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);//gera uma credencial

            var expiration = DateTime.UtcNow.AddMinutes(20); // expira em 20 minutos

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: creds);

            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration,
            };

        }
    }
}
