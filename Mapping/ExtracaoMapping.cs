using Dapper.FluentMap.Dommel.Mapping;
using NFCE.API.Models;

namespace NFCE.API.Mapping
{
    public class ExtracaoMapping : DommelEntityMap<ExtracaoModel>
    {
        public ExtracaoMapping()
        {
            ToTable("NFCET_CONTROLE");
            Map(x => x.Id).ToColumn("ID", caseSensitive: false).IsIdentity();
            Map(x => x.IdUsuario).ToColumn("ID_USR", caseSensitive: false);
            Map(x => x.ChaveAcesso).ToColumn("CHAVE_ACESSO", caseSensitive: false);
            Map(x => x.Tentativas).ToColumn("TENTATIVAS", caseSensitive: false);
            Map(x => x.Emissao).ToColumn("EMISSAO", caseSensitive: false);
            Map(x => x.URL).ToColumn("NFCE_URL", caseSensitive: false);
            Map(x => x.Status).ToColumn("ID_STATUS", caseSensitive: false);
            Map(x => x.Pagamento).Ignore();
            Map(x => x.Items).Ignore();
            Map(x => x.Emissor).Ignore();
            Map(x => x.Valido).Ignore();
        }
    }
}