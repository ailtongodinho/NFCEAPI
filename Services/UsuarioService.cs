using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using NFCE.API.Interfaces;
using NFCE.API.Interfaces.Repositories;
using NFCE.API.Interfaces.Services;
using NFCE.API.Models;
using NFCE.API.Models.Handler;
using NFCE.API.Models.Response.Usuario;

namespace NFCE.API.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IConfiguration _config;
        private static IUsuarioRepository _usuarioRepository;
        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _config = usuarioRepository.GetConfiguration;
            _usuarioRepository = usuarioRepository;
        }
        public UsuarioModel Consulta(int Id)
        {
            return _usuarioRepository.GetById(Id);
        }
        public UsuarioModel Consulta(UsuarioModel model)
        {
            // return _usuarioRepository.GetList(x => (x.RegistroNacional == model.RegistroNacional || x.Email == model.Email) && x.Ativo == true).FirstOrDefault();
            return _usuarioRepository.GetList(x => x.Email == model.Email && x.Ativo == true).FirstOrDefault();
        }
        public List<UsuarioModel> Listar()
        {
            return _usuarioRepository.GetAll().ToList();
        }
        public UsuarioNovoResponse Novo(UsuarioModel model)
        {
            bool sucesso = false;
            var usuario = Consulta(model);
            if (usuario != null) throw new HttpExceptionHandler("Usuário já existe!");

            sucesso = _usuarioRepository.Insert(model) > 0;

            return new UsuarioNovoResponse
            {
                Sucesso = sucesso,
                Mensagem = sucesso ? "Usuário criado com sucesso!" : "Aconteceu um erro ao criar o usuário",
                SenhaTemporaria = Guid.NewGuid().ToString()
            };
        }
        public UsuarioNovoResponse Atualizar(UsuarioModel model)
        {
            bool sucesso = false;
            var usuario = Consulta(model.Id);
            if (usuario == null) throw new HttpExceptionHandler("Usuário não existe!");

            sucesso = _usuarioRepository.UpdateGenerico(model, new { model.Email, model.Nome, model.Sexo, model.Sobrenome });

            return new UsuarioNovoResponse
            {
                Sucesso = sucesso,
                Mensagem = sucesso ? "Usuário atualizado com sucesso!" : "Aconteceu um erro ao atualizar o usuário"
            };
        }
    }
}