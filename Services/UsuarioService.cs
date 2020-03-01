using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using NFCE.API.Interfaces;
using NFCE.API.Models;

namespace NFCE.API.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IConfiguration _config;
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IConfiguration config, IUsuarioRepository usuarioRepository)
        {
            _config = config;
            _usuarioRepository = usuarioRepository;
        }
        public UsuarioModel Consulta(UsuarioModel model)
        {
            return _usuarioRepository.GetList(x => (x.RegistroNacional == model.RegistroNacional || x.Email == model.Email) && x.Ativo == true).FirstOrDefault();
        }
        public List<UsuarioModel> Listar()
        {
            return _usuarioRepository.GetAll().ToList();
        }
        public string Novo(UsuarioModel model)
        {
            bool sucesso = false;
            var usuario = Consulta(model);
            if(usuario == null)
            {
                sucesso = _usuarioRepository.Insert(model) > 0;
            }

            return sucesso ? "Usuário criado com sucesso!" : "Usuário já existe";
        }
    }
}