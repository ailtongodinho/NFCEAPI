using System;
using System.Collections.Generic;
using NFCE.API.Models;
using NFCE.API.Models.Request;
using NFCE.API.Models.Response.Emissor;

namespace NFCE.API.Interfaces.Repositories
{
    public interface IEmissorRepository : IRepositoryBase<EmissorModel>
    {
        int Adicionar(EmissorModel emissorModel);
        EmissorModel Consultar(int EmissorId);
        EmissorModel Consultar(EmissorModel emissorModel);
        IEnumerable<EmissorAgregadoResponse> Agregado(int IdUsuario, EmissorAgregadoRequest emissorAgregadoRequest);
    }
}