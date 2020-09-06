using Dapper.FluentMap.Dommel.Mapping;
using NFCE.API.Models.Saldos;

namespace NFCE.API.Mapping
{
    public class SaldosMapping : DommelEntityMap<SaldosModel>
    {
        public SaldosMapping()
        {
            ToTable("NFCET_SALDOS");
            Map(x => x.IdProduto).ToColumn("ID_PRODUTO", false);
            Map(x => x.IdEmissor).ToColumn("ID_EMISSOR", false);
            Map(x => x.ValorUnitario).ToColumn("VLR_UN", false);
            Map(x => x.DataInsercao).ToColumn("DATA_INSERCAO", false);
        }
    }
}