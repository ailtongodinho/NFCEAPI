using System;
using System.Net;

namespace NFCE.API.Models
{
    public class ResponseModel
    {
        public bool Sucesso { get; set; }
        public HttpStatusCode HttpStatus { get; set; }
        public string Mensagem { get; set; }
        public DateTime DataHora => DateTime.Now;
        public object Objeto { get; set; }
        public ResponseModel ()
        {
            Sucesso = true;
            HttpStatus = HttpStatusCode.OK;
        }
    }
}