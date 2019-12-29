using System;
using Microsoft.Extensions.Configuration;
using NFCE.API.Interfaces;
using NFCE.API.Models;

namespace NFCE.API.Services
 {
     public class AuthService : IAuthService
     {
         private readonly IConfiguration config;
         public AuthService(IConfiguration _config)
         {
             config = _config;
         }
         public string Login(AuthModel usuario)
         {
             return $"Usuário: {usuario.Login} | Senha: {usuario.Senha}";
         }
     }
 }