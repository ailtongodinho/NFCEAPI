using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NFCE.API.Extensions;
using NFCE.API.Interfaces.Repositories;
using NFCE.API.Interfaces.Services;
using NFCE.API.Models;
using NFCE.API.Models.Handler;
using NFCE.API.Models.Request;
using NFCE.API.Models.Request.Usuario;
using NFCE.API.Models.Response.Usuario;

namespace NFCE.API.Services
{
    public class AuthService : IAuthService
    {
        public static UsuarioModel UsuarioAtual { get; set; }
        private readonly IConfiguration _config;
        private readonly IAuthRepository _authRepository;
        private readonly IUsuarioService _usuarioService;
        private readonly SigningExtension _signingExtension;
        public AuthService(IUsuarioService usuarioService, SigningExtension signingExtension, IAuthRepository authRepository)
        {
            _config = authRepository.GetConfiguration;
            _usuarioService = usuarioService;
            _signingExtension = signingExtension;
            _authRepository = authRepository;
        }
        public UsuarioLoginResponse AlterarSenha(int idUsuario, AuthAlterarSenha authAlterarSenha)
        {
            //  Consulta usuário
            var auth = _authRepository.Consulta(new AuthModel { IdUsuario = idUsuario });
            if (auth == null) throw new HttpExceptionHandler("Usuário inválido!");
            if (auth.Senha == authAlterarSenha.SenhaNova) throw new HttpExceptionHandler("Senha deve ser diferente da anterior");
            //  Atualizando registro
            auth.Senha = authAlterarSenha.SenhaNova;
            _authRepository.UpdateGenerico(auth, new { auth.Senha }, new { auth.Id });
            return new UsuarioLoginResponse { Message = "Senha alterada com sucesso" };
        }
        public UsuarioLoginResponse Login(AuthModel usuario)
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
                if (usuarioBase == null) throw new HttpExceptionHandler(mensagem, HttpStatusCode.BadRequest);

                usuario.IdUsuario = usuarioBase.Id;
                usuario.DataUltimo = DateTime.Now;
                //  Verificando senha
                autentificacao = false;
                var auth = _authRepository.Consulta(usuario);
                mensagem = "OK";
                if (auth == null)
                {
                    mensagem = "Login craido com sucesso!";
                    //  Criando novo Login
                    autentificacao = _authRepository.Insert(usuario) > 0;
                }
                else if (auth.Valido)
                {
                    usuario.Id = auth.Id;
                    //  Atualizando Registro
                    autentificacao = _authRepository.UpdateGenerico(usuario, new { usuario.DataUltimo }, new { usuario.Id });
                }
                //  Usuário é autentico?
                if (!autentificacao) throw new HttpExceptionHandler(mensagem, HttpStatusCode.BadRequest);
                //  Atualizando usuário Atual
                UsuarioAtual = usuarioBase;
                //  Criando Token JWT
                ClaimsIdentity identity = new ClaimsIdentity(new Claim[]
                    // new GenericIdentity(usuario.Login, "Login"),
                    {
                                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                                    new Claim(JwtRegisteredClaimNames.UniqueName, usuario.IdUsuario.ToString()),
                                    new Claim(JwtRegisteredClaimNames.NameId, usuario.Login),
                                    // new Claim(ClaimTypes.Name, usuario.Id.ToString()),
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

            }

            return new UsuarioLoginResponse
            {
                Authenticated = autentificacao,
                Created = dataCriacao,
                Expiration = dataExpiracao,
                AccessToken = token,
                Message = mensagem
            };
        }
    }
}