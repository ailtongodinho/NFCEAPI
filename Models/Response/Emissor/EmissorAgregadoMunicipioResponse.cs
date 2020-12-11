using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace NFCE.API.Models.Response.Emissor
{
    public class EmissorAgregadoMunicipioResponse
    {
        public string Municipio { get; set; }
        public IEnumerable<EmissorAgregadoResponse> Dados { get; set; }
    }
}