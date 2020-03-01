using System.Collections.Generic;
using NFCE.API.Models;
using NFCE.API.Models.Request;
using NFCE.API.Models.Response;

namespace NFCE.API.Interfaces
{
    public interface IExtracaoService
    {
        BaseResponseModel ProcessarNFCE(ExtracaoRequestModel extracaoRequestModel);
        ExtracaoListarResponse Listar(ExtracaoListarRequest extracaoListarRequest);
    }
}