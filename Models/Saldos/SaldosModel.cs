using System;

namespace NFCE.API.Models.Saldos
{
    public class SaldosModel
    {
        public int IdProduto { get; set; }
        public int IdEmissor { get; set; }
        public decimal ValorUnitario { get; set; }
        public DateTime DataInsercao { get; set; }
        public EmissorModel Emissor { get; set; }
    }
}