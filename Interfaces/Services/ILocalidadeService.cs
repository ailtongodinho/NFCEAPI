using System.Collections.Generic;
using NFCE.API.Models;
using NFCE.API.Models.IBGE;

namespace NFCE.API.Interfaces.Services
{
    public interface ILocalidadeService
    {
        long Novo(LocalidadeModel model);
        LocalidadeModel Consulta(int Id);
        LocalidadeModel Consulta(LocalidadeModel model);
        List<LocalidadeModel> Listar();
        bool Atualizar(LocalidadeModel model);
        LocalidadeModel Consulta(List<EmissorModel> models);
        List<IbgeEstado> ListarEstados();
        List<IbgeDistrito> ListarDistritos(int municipioId);
        List<IbgeMunicipio> ListarMunicipios(int uFId);
    }
}