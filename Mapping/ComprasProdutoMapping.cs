using Dapper.FluentMap.Dommel.Mapping;
using NFCE.API.Models;

namespace NFCE.API.Mapping
{
    public class ComprasProdutoMapping : DommelEntityMap<ComprasProdutoModel>
    {
        public ComprasProdutoMapping()
        {
            ToTable("NFCET_COMPRAS_PRODUTO");
            Map(x => x.Apelido).ToColumn("APELIDO", false);
            Map(x => x.DataCriacao).ToColumn("DATA_INSERCAO", false);
            Map(x => x.Id).ToColumn("ID", false).IsIdentity();
            Map(x => x.IdCompra).ToColumn("ID_COMPRA", false);
            Map(x => x.IdProduto).ToColumn("ID_PRODUTO", false);
            Map(x => x.Produto).Ignore();
            Map(x => x.Quantidade).ToColumn("QTD", false);
            Map(x => x.Unidade).ToColumn("UNIDADE", false);
            Map(x => x.Total).Ignore();
        }
    }
}