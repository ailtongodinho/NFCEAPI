using System.Collections.Generic;
using System.Linq;
using NFCE.API.Interfaces;
using NFCE.API.Interfaces.Services;
using NFCE.API.Models.Request;
using NFCE.API.Models.Response.Emissor;

namespace NFCE.API.Services
{
    public class AgregadoService : IAgregadoService
    {
        private readonly INotaService _NotaService;
        private readonly IItemService _ItemService;
        private readonly IPagamentoService _PagamentoService;
        private readonly IEmissorService _EmissorService;
        public AgregadoService(
            IItemService ItemService,
            IPagamentoService PagamentoService,
            IEmissorService EmissorService,
            INotaService NotaService
        )
        {
            _NotaService = NotaService;
            _PagamentoService = PagamentoService;
            _EmissorService = EmissorService;
            _ItemService = ItemService;
        }
        // public IEnumerable<EmissorAgregadoResponse> Emissor(int idUsuario, EmissorAgregadoRequest emissorAgregadoRequest)
        public object Emissor(int idUsuario, EmissorAgregadoRequest emissorAgregadoRequest)
        {
            var agregado = _EmissorService.Agregado(idUsuario, emissorAgregadoRequest).ToList();
            // agregado.ForEach(x => {
            //     x.Pagamento = _PagamentoService.Consultar(x.UltimoPagamento);
            //     // x.Nota = _NotaService.Consultar(x.UltimaNota);
            // });
            var agrupado = (
                from c in agregado
                group c by c.UF into newGroup
                orderby newGroup.Key
                select new EmissorAgregadoUFResponse {
                    UF = newGroup.Key,
                    Dados = 
                        newGroup
                            .GroupBy(x => x.Municipio)
                            .Select(x => new EmissorAgregadoMunicipioResponse {
                                Municipio = x.Key,
                                Dados = x.Where(y => y.Municipio == x.Key)
                            })
                            .OrderBy(x => x.Municipio)
                });

            return agrupado;
        }
    }
}