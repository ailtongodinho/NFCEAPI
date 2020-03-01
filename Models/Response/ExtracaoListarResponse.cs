using System.Collections.Generic;

namespace NFCE.API.Models.Response
{
    public class ExtracaoListarResponse
    {
        public IEnumerable<ExtracaoModel> Dados { get; set; }
        public long Quantidade { get; set; }
        public decimal Total { get; set; }
    }
}