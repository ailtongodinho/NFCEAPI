using System.Collections.Generic;
using NFCE.API.Models.Request;
using NFCE.API.Models.Response.Emissor;

namespace NFCE.API.Interfaces
{
    public interface IAgregadoService
    {
        // IEnumerable<EmissorAgregadoResponse> Emissor(int idUsuario, EmissorAgregadoRequest emissorAgregadoRequest);
        object Emissor(int idUsuario, EmissorAgregadoRequest emissorAgregadoRequest);
    }
}