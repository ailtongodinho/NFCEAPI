using Dapper.FluentMap.Dommel.Mapping;
using NFCE.API.Models;

namespace NFCE.API.Mapping
{
    public class EmissorMapping : DommelEntityMap<EmissorModel>
    {
        public EmissorMapping()
        {
            ToTable("NFCET_EMISSOR");
            Map(x => x.Id).ToColumn("ID", caseSensitive: false).IsIdentity();
            Map(x => x.IdLocalidade).ToColumn("ID_LOCALIDADE", caseSensitive: false);
            Map(x => x.CEP).ToColumn("CEP", caseSensitive: false);
            Map(x => x.CNPJ).ToColumn("CNPJ", caseSensitive: false);
            Map(x => x.Distrito).ToColumn("DISTRITO", caseSensitive: false);
            Map(x => x.Logradouro).ToColumn("LOGRADOURO", caseSensitive: false);
            Map(x => x.Municipio).ToColumn("MUNICIPIO", caseSensitive: false);
            Map(x => x.Numero).ToColumn("NUMERO", caseSensitive: false);
            Map(x => x.RazaoSocial).ToColumn("RAZAO_SOCIAL", caseSensitive: false);
            Map(x => x.UF).ToColumn("UF", caseSensitive: false);
            Map(x => x.NomeFantasia).ToColumn("NOME_FANTASIA", caseSensitive: false);
            Map(x => x.DataInsercao).ToColumn("DATA_INSERCAO", caseSensitive: false);
            Map(x => x.Telefone).Ignore();
            Map(x => x.Valido).Ignore();
            Map(x => x.InscricaoEstadual).Ignore();
            Map(x => x.Endereco).Ignore();
        }
    }
}