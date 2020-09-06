using Dapper.FluentMap.Dommel.Mapping;
using NFCE.API.Models;

namespace NFCE.API.Mapping
{
    public class PagamentoMapping : DommelEntityMap<PagamentoModel>
    {
        public PagamentoMapping()
        {
            ToTable("NFCET_PAG");
            Map(x => x.Id).ToColumn("ID", false).IsIdentity();
            Map(x => x.IdControle).ToColumn("ID_CTRL", false);
            Map(x => x.FormaPagamento).ToColumn("FORMA_PGTO", false);
            Map(x => x.TributosTotaisIncidentes).ToColumn("TRIBUTOS", false);
            Map(x => x.Troco).ToColumn("TROCO", false);
            Map(x => x.ValorPago).ToColumn("VLR_PAGO", false);
            Map(x => x.Valido).Ignore();
            Map(x => x.MeioPagamento).Ignore();
        }
    }
}