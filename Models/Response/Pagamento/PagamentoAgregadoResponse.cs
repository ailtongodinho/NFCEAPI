using System.Collections.Generic;
using System.Linq;

namespace NFCE.API.Models.Response.Pagamento
{
    public class PagamentoAgregadoResponse
    {
        /// <summary>
        /// Quantidade
        /// </summary>
        /// <value>Ex: 10</value>
        public long Quantidade { get; set; }
        /// <summary>
        /// Total Pago
        /// </summary>
        /// <value>10.00</value>
        public decimal TotalTroco { get; set; }
        /// <summary>
        /// Total Pago
        /// </summary>
        /// <value>10.00</value>
        public decimal Total { get; set; }
        /// <summary>
        /// Total Impostos
        /// </summary>
        /// <value>10.00</value>
        public decimal TotalTributos { get; set; }
    }
}