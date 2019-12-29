using NFCE.API.Models;

namespace NFCE.API.Interfaces
{
    public interface IExtracaoService
    {
        BaseResponseModel ProcessarNFCE(string _URL);
    }
}