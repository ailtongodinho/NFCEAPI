using System.Linq;
using Microsoft.Extensions.Configuration;
using NFCE.API.Interfaces.Repositories;
using NFCE.API.Models;

namespace NFCE.API.Repositories
{
    public class AuthRepository : RepositoryBase<AuthModel>, IAuthRepository
    {
        public AuthRepository(IConfiguration config) : base(config)
        {
            
        }
        public AuthModel Consulta(AuthModel model)
        {
            var lista = GetList(x => x.IdUsuario == model.IdUsuario);
            var retorno = lista.Count() > 0 ? lista.First() : null;
            if(retorno != null){
                retorno.Valido = retorno.Senha == model.Senha;
            }
            return retorno;
        }
        
    }
}