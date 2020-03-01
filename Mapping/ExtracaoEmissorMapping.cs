using Dapper.FluentMap.Dommel.Mapping;
using NFCE.API.Models;

namespace NFCE.API.Mapping
{
    public class ExtracaoEmissorMapping : DommelEntityMap<ExtracaoEmissorModel>
    {
        public ExtracaoEmissorMapping()
        {
            ToTable("NFCET_EMISSOR");
            Map(x => x.Id).ToColumn("ID", caseSensitive: false).IsIdentity();
            Map(x => x.IdControle).ToColumn("ID_CTRL", caseSensitive: false);
            Map(x => x.CEP).ToColumn("CEP", caseSensitive: false);
            Map(x => x.CNPJ).ToColumn("CNPJ", caseSensitive: false);
            Map(x => x.Distrito).ToColumn("DISTRITO", caseSensitive: false);
            Map(x => x.Logradouro).ToColumn("LOGRADOURO", caseSensitive: false);
            Map(x => x.Municipio).ToColumn("MUNICIPIO", caseSensitive: false);
            Map(x => x.Numero).ToColumn("NUMERO", caseSensitive: false);
            Map(x => x.RazaoSocial).ToColumn("RAZAO_SOCIAL", caseSensitive: false);
            Map(x => x.UF).ToColumn("UF", caseSensitive: false);
            Map(x => x.Valido).Ignore();
            Map(x => x.InscricaoEstadual).Ignore();
            Map(x => x.Endereco).Ignore();
        }
    }
}