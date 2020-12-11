using System.Collections.Generic;
using System.Linq;
using NFCE.API.Models.Response.Item;
using NFCE.API.Models.Response.Pagamento;

namespace NFCE.API.Models.Response.Nota
{
    public class NotaAgregadoResponse
    {
        public ItemAgregadoResponse ItemAgregado { get; set; }
        public PagamentoAgregadoResponse PagamentoAgregado { get; set; }
    }
}