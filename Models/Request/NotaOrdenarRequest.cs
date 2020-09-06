using System.Collections.Generic;
using NFCE.API.Models.Response.Nota;

namespace NFCE.API.Models.Request
{
    public class NotaOrdenarRequest
    {
        public List<NotaListarResponse> ListaAgrupada { get; set; }
        public List<NotaModel> Lista { get; set; }
        public string OrdernarId { get; set; }
    }
}