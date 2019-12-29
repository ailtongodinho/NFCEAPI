using System;

namespace NFCE.API.Models
{
    public class BaseResponseModel
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public object Data { get; set; }
        public DateTime DataHora => DateTime.Now;
    }
}