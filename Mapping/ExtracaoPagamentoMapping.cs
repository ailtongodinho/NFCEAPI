using Dapper.FluentMap.Dommel.Mapping;
using NFCE.API.Models;

namespace NFCE.API.Mapping
{
    public class ExtracaoPagamentoMapping : DommelEntityMap<ExtracaoPagamentoModel>
    {
        public ExtracaoPagamentoMapping()
        {
            ToTable("NFCET_PAG");
            Map(x => x.Id).ToColumn("ID", true).IsIdentity();
            Map(x => x.IdControle).ToColumn("ID_CTRL", true);
            Map(x => x.FormaPagamento).ToColumn("FORMA_PGTO", true);
            Map(x => x.TributosTotaisIncidentes).ToColumn("TRIBUTOS", true);
            Map(x => x.Troco).ToColumn("TROCO", true);
            Map(x => x.ValorPago).ToColumn("VLR_PAGO", true);
            Map(x => x.Valido).Ignore();
        }
    }
}