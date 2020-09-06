using System.Collections.Generic;
using System.Linq;

namespace NFCE.API.Models.Response.Item
{
    public class ItemAgregadoResponse
    {
        /// <summary>
        /// Ranking de Itens
        /// </summary>
        /// <value></value>
        public List<ItemRankingResponse> Ranking { get; set; }
        /// <summary>
        /// Quantidade de Itens
        /// </summary>
        /// <value>Ex: 10</value>
        public long Quantidade { get; set; }
        /// <summary>
        /// Total Gasto
        /// </summary>
        /// <value>10.00</value>
        public decimal Total { get; set; }
    }
}