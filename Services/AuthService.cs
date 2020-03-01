using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NFCE.API.Extensions;
using NFCE.API.Interfaces;
using NFCE.API.Models;
using NFCE.API.Repositories;

namespace NFCE.API.Services
{
    public class AuthService : IAuthService
    {
        public static UsuarioModel UsuarioAtual { get; set; }
        private readonly IConfiguration _config;
        private readonly IAuthRepository _authRepository;
        private readonly IUsuarioService _usuarioService;
        private readonly SigningExtension _signingExtension;
        public AuthService(IConfiguration config, IUsuarioService usuarioService, SigningExtension signingExtension, IAuthRepository authRepository)
        {
            _config = config;
            _usuarioService = usuarioService;
            _signingExtension = signingExtension;
            _authRepository = authRepository;
        }
        public object Login(AuthModel usuario)
        {
            bool autentificacao = false;
            string mensagem = "Login/Senha Incorretos";
            string token = null;
            DateTime? dataCriacao = null;
            DateTime? dataExpiracao = null;
            if (usuario != null && !string.IsNullOrWhiteSpace(usuario.Login))
            {
                //  Verificando usuário
                var usuarioBase = _usuarioService.Consulta(new UsuarioModel { RegistroNacional = usuario.Login, Email = usuario.Login });
                if (usuarioBase != null)
                {
                    usuario.IdUsuario = usuarioBase.Id;
                    usuario.DataUltimo = DateTime.Now;
                    //  Verificando senha
                    bool valido = false;
                    var auth = _authRepository.Consulta(usuario);
                    if (auth == null)
                    {
                        //  Criando novo Login
                        valido = _authRepository.Insert(usuario) > 0;
                    }
                    else if (auth.Valido)
                    {
                        usuario.Id = auth.Id;
                        //  Atualizando Registro
                        valido = _authRepository.UpdateGenerico(usuario, new { usuario.DataUltimo }, new { usuario.Id });
                    }
                    //  Usuário é autentico?
                    if (valido)
                    {
                        //  Atualizando usuário Atual
                        UsuarioAtual = usuarioBase;
                        //  Criando Token JWT
                        ClaimsIdentity identity = new ClaimsIdentity(new Claim[]
                            // new GenericIdentity(usuario.Login, "Login"),
                            {
                                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                                    new Claim(JwtRegisteredClaimNames.UniqueName, usuario.Login),
                                    new Claim(JwtRegisteredClaimNames.NameId, usuario.Id.ToString()),
                                    // new Claim(ClaimTypes.Name, usuario.Login),
                                    new Claim(ClaimTypes.Role, "Admin")
                            }
                        );

                        dataCriacao = DateTime.Now;
                        dataExpiracao = dataCriacao + TimeSpan.FromSeconds(_config.GetValue<int>("TokenConfiguration:Seconds"));

                        var handler = new JwtSecurityTokenHandler();
                        var securityToken = handler.CreateToken(new SecurityTokenDescriptor
                        {
                            Issuer = _config.GetValue<string>("TokenConfiguration:Issuer"),
                            Audience = _config.GetValue<string>("TokenConfiguration:Audience"),
                            SigningCredentials = _signingExtension.Credenciais,
                            Subject = identity,
                            NotBefore = dataCriacao,
                            Expires = dataExpiracao
                        });
                        token = handler.WriteToken(securityToken);
                        mensagem = "OK";
                    }
                }
            }

            return new
            {
                authenticated = autentificacao,
                created = dataCriacao?.ToString("yyyy-MM-dd HH:mm:ss"),
                expiration = dataExpiracao?.ToString("yyyy-MM-dd HH:mm:ss"),
                accessToken = token,
                message = mensagem
            };
        }
    }
}