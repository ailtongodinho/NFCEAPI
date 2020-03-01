using System;

namespace NFCE.API.Models.Request
{
    public class ExtracaoListarRequest
    {
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
    }
}