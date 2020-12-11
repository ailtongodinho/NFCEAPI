using System;
using System.Collections.Generic;
using NFCE.API.Models;
using NFCE.API.Models.Request;
using NFCE.API.Models.Response;

namespace NFCE.API.Interfaces.Services
{
    public interface IExtracaoService
    {
        object Processar(int idUsuario, ExtracaoRequest extracaoRequestModel);
        string Consultar(string chaveAcesso);
    }
}