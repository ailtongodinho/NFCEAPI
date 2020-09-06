using System;
using Newtonsoft.Json;

namespace NFCE.API.Models.Response
{
    public class PickerResponse
    {
        public string Id { get; set; }
        public string Descricao { get; set; }
        public string Texto { get; set; }
        public object Valor { get; set; }
        public PickerResponse(string texto, object valor, string id = null, string descricao = null)
        {
            Id = id ?? valor.ToString();
            Texto = texto;
            Descricao = descricao ?? string.Empty;
            Valor = valor;
        }
    }
}