using Dapper.FluentMap.Dommel.Mapping;
using NFCE.API.Models;

namespace NFCE.API.Mapping
{
    public class ProdutoMapping : DommelEntityMap<ProdutoModel>
    {
        public ProdutoMapping()
        {
            ToTable("NFCET_PRODUTOS");
            Map(x => x.Apelido).ToColumn("APELIDO", false);
            Map(x => x.CEST).ToColumn("CEST", false);
            Map(x => x.CFOP).ToColumn("CFOP", false);
            Map(x => x.Codigo).ToColumn("CODIGO", false);
            Map(x => x.DataCriacao).ToColumn("DATA_INSERCAO", false);
            Map(x => x.DataSaldos).ToColumn("DATA_SALDOS", false);
            Map(x => x.EAN).ToColumn("EAN", false);
            Map(x => x.Id).ToColumn("ID", false).IsIdentity();
            Map(x => x.IdEmissor).ToColumn("ID_EMISSOR", false);
            Map(x => x.NCM).ToColumn("NCM", false);
            Map(x => x.Nome).ToColumn("NOME", false);
            Map(x => x.Unidade).ToColumn("UNIDADE", false);
            // Map(x => x.ValorUnitario).Ignore();
        }
    }
}