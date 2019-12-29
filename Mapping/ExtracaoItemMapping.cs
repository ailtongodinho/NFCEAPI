using Dapper.FluentMap.Dommel.Mapping;
using NFCE.API.Models;

namespace NFCE.API.Mapping
{
    public class ExtracaoItemMapping : DommelEntityMap<ExtracaoItemModel>
    {
        public ExtracaoItemMapping()
        {
            ToTable("NFCET_ITEM");
            Map(x => x.Id).IsIdentity();
            Map(x => x.IdControle).ToColumn("ID_CTRL");
            Map(x => x.Nome).ToColumn("NOME");
            Map(x => x.Quantidade).ToColumn("QTD");
            Map(x => x.Unidade).ToColumn("UNIDADE");
            Map(x => x.ValorUnitario).ToColumn("VLR_UN");
            Map(x => x.ValorTotal).ToColumn("VLR_TOT");
            Map(x => x.Valido).Ignore();
        }
    }
}