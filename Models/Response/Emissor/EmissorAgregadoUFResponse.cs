using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace NFCE.API.Models.Response.Emissor
{
    public class EmissorAgregadoUFResponse
    {
        public string UF { get; set; }
        public IEnumerable<EmissorAgregadoMunicipioResponse> Dados { get; set; }
    }
}