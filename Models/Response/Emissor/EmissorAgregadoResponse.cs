using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace NFCE.API.Models.Response.Emissor
{
    public class EmissorAgregadoResponse : EmissorModel
    {
        /// <summary>
        /// Quantidade de Notas Fiscais
        /// </summary>
        /// <value>Ex: 10</value>
        public int QuantidadeNotas { get; set; }
        /// <summary>
        /// Total Gasto
        /// </summary>
        /// <value>10.00</value>
        public decimal SomatoriaTotal { get; set; }
        /// <summary>
        /// Total Gasto
        /// </summary>
        /// <value>10.00</value>
        public decimal SomatoriaTroco { get; set; }
        /// <summary>
        /// Total Gasto
        /// </summary>
        /// <value>10.00</value>
        public decimal SomatoriaTributos { get; set; }
    }
}