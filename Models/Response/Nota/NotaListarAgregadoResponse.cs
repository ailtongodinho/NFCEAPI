using System;
using System.Collections.Generic;
using System.Linq;

namespace NFCE.API.Models.Response.Nota
{
    public class NotaListarAgregadoResponse
    {
        public IEnumerable<NotaListarResponse> Lista { get; set; }
        public DateTime DataMinima { get; set; }
        public DateTime DataMaxima { get; set; }
        public int IdMinimo { get; set; }
        public int IdMaximo { get; set; }
        public bool Ultimo { get; set; }
        public NotaListarAgregadoResponse(IEnumerable<NotaListarResponse> lista)
        {
            Lista = lista;
            if (lista.Count() > 0)
            {
                IdMinimo = Lista.Min(x => x.IdMinimo);
                IdMaximo = Lista.Max(x => x.IdMaximo);
                DataMaxima = Lista.Min(x => x.DataMaxima);
                DataMinima = Lista.Min(x => x.DataMinima);
            }
        }
    }
}