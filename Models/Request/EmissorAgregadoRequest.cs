using System;

namespace NFCE.API.Models.Request
{
    public class EmissorAgregadoRequest
    {
        public DateTime? DataInicial { get; set; }
        public DateTime? DataFinal { get; set; }
        public EmissorAgregadoRequest() { }
        public EmissorAgregadoRequest(DateTime? dataInicial, DateTime? dataFinal)
        {
            DataFinal = dataFinal;
            DataInicial = dataInicial;
        }
    }
}