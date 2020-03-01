using Dapper.FluentMap.Dommel.Mapping;
using NFCE.API.Models;

namespace NFCE.API.Mapping
{
    public class ExtracaoItemMapping : DommelEntityMap<ExtracaoItemModel>
    {
        public ExtracaoItemMapping()
        {
            ToTable("NFCET_ITEM");
            Map(x => x.Id).ToColumn("ID", caseSensitive: false).IsIdentity();
            Map(x => x.IdControle).ToColumn("ID_CTRL", caseSensitive: false);
            Map(x => x.Nome).ToColumn("NOME", caseSensitive: false);
            Map(x => x.Quantidade).ToColumn("QTD", caseSensitive: false);
            Map(x => x.Unidade).ToColumn("UNIDADE", caseSensitive: false);
            Map(x => x.ValorUnitario).ToColumn("VLR_UN", caseSensitive: false);
            Map(x => x.ValorTotal).ToColumn("VLR_TOT", caseSensitive: false);
            Map(x => x.Valido).Ignore();
        }
    }
}