using System.Collections.Generic;
using NFCE.API.Models;

namespace NFCE.API.Interfaces
{
    public interface IUsuarioService
    {
        string Novo(UsuarioModel model);
        UsuarioModel Consulta(UsuarioModel model);
        List<UsuarioModel> Listar();
    }
}