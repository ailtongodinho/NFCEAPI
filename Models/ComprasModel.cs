using System;

namespace NFCE.API.Models
{
    public class ComprasModel
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public string Nome { get; set; }
        public long IdLocalidade { get; set; }
        public decimal Total { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataModificacao { get; set; }
    }
}