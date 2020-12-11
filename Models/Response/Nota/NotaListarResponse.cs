using System;
using System.Collections.Generic;
using System.Linq;

namespace NFCE.API.Models.Response.Nota
{
    public class NotaListarResponse
    {
        public DateTime Data { get; set; }
        public IEnumerable<NotaModel> Dados { get; set; }
        public long Quantidade { get { return Dados.Count(); } }
        public decimal Total { get { return Dados.Sum(x => x.ValorTotal); } }
        public DateTime DataMinima { get { return Dados.Min(x => x.Emissao); } }
        public DateTime DataMaxima { get { return Dados.Max(x => x.Emissao); } }
        public int IdMinimo { get { return Dados.Min(x => x.Id); } }
        public int IdMaximo { get { return Dados.Max(x => x.Id); } }
        public NotaListarResponse(List<NotaModel> dados)
        {
            Dados = dados;
        }
        public NotaListarResponse(DateTime data, IEnumerable<NotaModel> dados)
        {
            Data = data;
            Dados = dados;
        }
    }
}