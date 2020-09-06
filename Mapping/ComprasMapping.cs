using Dapper.FluentMap.Dommel.Mapping;
using NFCE.API.Models;

namespace NFCE.API.Mapping
{
    public class ComprasMapping : DommelEntityMap<ComprasModel>
    {
        public ComprasMapping()
        {
            ToTable("NFCET_COMPRAS");
            Map(x => x.Id).ToColumn("ID", caseSensitive: false).IsIdentity();
            Map(x => x.IdLocalidade).ToColumn("ID_LOCALIDADE", caseSensitive: false);
            Map(x => x.IdUsuario).ToColumn("ID_USR", caseSensitive: false);
            Map(x => x.DataCriacao).ToColumn("DATA_INSERCAO", caseSensitive: false);
            Map(x => x.DataModificacao).ToColumn("DATA_MODIFICACAO", caseSensitive: false);
            Map(x => x.Nome).ToColumn("NOME", caseSensitive: false);
            Map(x => x.Total).ToColumn("VLR_TOTAL", caseSensitive: false);
        }
    }
}