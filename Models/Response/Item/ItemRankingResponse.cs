using System;
using System.Collections.Generic;
using System.Linq;

namespace NFCE.API.Models.Response.Item
{
    public class ItemRankingResponse
    {
        /// <summary>
        /// Ranking de Itens
        /// </summary>
        /// <value>Ex: 10</value>
        public string Nome { get; set; }
        /// <summary>
        /// Valor máximo do Item
        /// </summary>
        /// <value>Ex: 10</value>
        public decimal ValorMax { get; set; }
        /// <summary>
        /// Valor mínimo do Item
        /// </summary>
        /// <value>Ex: 10</value>
        public decimal ValorMin { get; set; }
        /// <summary>
        /// Total Gasto
        /// </summary>
        /// <value>10.00</value>
        public decimal Total { get; set; }
        /// <summary>
        /// Última compra
        /// </summary>
        /// <value>10.00</value>
        public DateTime UltimaCompra { get; set; }
        /// <summary>
        /// Último emissor
        /// </summary>
        /// <value>10.00</value>
        public EmissorModel UltimoEmissor { get; set; }
    }
}