using System;

namespace NFCE.API.Models.Request
{
    public class NotaListarRequest
    {
        public bool? Favorito { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public string OrdernarId { get; set; }
        public int? IdEmissor { get; set; }
        public int? Top { get; set; }
        public int? Id { get; set; }
    }
}