using System.Collections.Generic;
using NFCE.API.Models;
using NFCE.API.Models.Response.Usuario;

namespace NFCE.API.Interfaces.Services
{
    public interface IUsuarioService
    {
        UsuarioNovoResponse Novo(UsuarioModel model);
        UsuarioModel Consulta(int Id);
        UsuarioModel Consulta(UsuarioModel model);
        List<UsuarioModel> Listar();
        UsuarioNovoResponse Atualizar(UsuarioModel model);
    }
}