using Dapper.FluentMap.Dommel.Mapping;
using NFCE.API.Models;

namespace NFCE.API.Mapping
{
    public class ExtracaoEmissorMapping : DommelEntityMap<ExtracaoEmissorModel>
    {
        public ExtracaoEmissorMapping()
        {
            ToTable("NFCET_EMISSOR");
            Map(x => x.Id).ToColumn("ID", true).IsIdentity();
            Map(x => x.IdControle).ToColumn("ID_CTRL", true);
            Map(x => x.CEP).ToColumn("CEP", true);
            Map(x => x.CNPJ).ToColumn("CNPJ", true);
            Map(x => x.Distrito).ToColumn("DISTRITO", true);
            Map(x => x.Logradouro).ToColumn("LOGRADOURO", true);
            Map(x => x.Municipio).ToColumn("MUNICIPIO", true);
            Map(x => x.Numero).ToColumn("NUMERO", true);
            Map(x => x.RazaoSocial).ToColumn("RAZAO_SOCIAL", true);
            Map(x => x.UF).ToColumn("UF", true);
            Map(x => x.Valido).Ignore();
            Map(x => x.InscricaoEstadual).Ignore();
            Map(x => x.Endereco).Ignore();
        }
    }
}