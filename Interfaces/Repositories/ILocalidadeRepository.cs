using System.Collections.Generic;
using NFCE.API.Models;
using NFCE.API.Models.IBGE;

namespace NFCE.API.Interfaces.Repositories
{
    public interface ILocalidadeRepository : IRepositoryBase<LocalidadeModel>
    {
        List<IbgeEstado> ListarEstados();
        List<IbgeMunicipio> ListarMunicipios(int uFId);
        List<IbgeDistrito> ListarDistritos(int municipioId);
    }
}