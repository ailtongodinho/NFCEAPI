using System;
using System.Net;

namespace NFCE.API.Models.Handler
{
    public class HttpExceptionHandler : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public HttpExceptionHandler(string mensagemUsuario, HttpStatusCode statusCode , Exception innerException = null) : this(mensagemUsuario, innerException)
        {
            StatusCode = statusCode;
        }
        public HttpExceptionHandler(string mensagemUsuario, Exception innerException = null) : base(mensagemUsuario, innerException)
        {
            StatusCode = HttpStatusCode.InternalServerError;
        }
    }
}