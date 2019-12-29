using Dapper.FluentMap.Dommel.Mapping;
using NFCE.API.Models;

namespace NFCE.API.Mapping
{
    public class ExtracaoMapping : DommelEntityMap<ExtracaoModel>
    {
        public ExtracaoMapping()
        {
            ToTable("NFCET_CONTROLE");
            Map(x => x.Id).ToColumn("ID").IsIdentity();
            Map(x => x.IdUsuario).ToColumn("ID_USR");
            Map(x => x.ChaveAcesso).ToColumn("CHAVE_ACESSO");
            Map(x => x.Tentativas).ToColumn("TENTATIVAS");
            Map(x => x.Emissao).ToColumn("EMISSAO");
            Map(x => x.URL).ToColumn("NFCE_URL");
            Map(x => x.Status).ToColumn("ID_STATUS");
            Map(x => x.Pagamento).Ignore();
            Map(x => x.Items).Ignore();
            Map(x => x.Emissor).Ignore();
            Map(x => x.Valido).Ignore();
        }
    }
}