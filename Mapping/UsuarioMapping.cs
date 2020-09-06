using Dapper.FluentMap.Dommel.Mapping;
using NFCE.API.Models;

namespace NFCE.API.Mapping
{
    public class UsuarioMapping : DommelEntityMap<UsuarioModel>
    {
        public UsuarioMapping()
        {
            ToTable("NFCET_USR");
            Map(x => x.Id).ToColumn("ID", false).IsIdentity();
            Map(x => x.Nome).ToColumn("NOME", false);
            Map(x => x.Sobrenome).ToColumn("SOBRENOME", false);
            Map(x => x.RegistroNacional).ToColumn("REGISTRO_NACIONAL", false);
            Map(x => x.Sexo).ToColumn("SEXO", false);
            Map(x => x.Email).ToColumn("EMAIL", false);
            Map(x => x.Ativo).ToColumn("ATIVO", false);
            Map(x => x.UF).Ignore();
            Map(x => x.Municipio).Ignore();
            Map(x => x.CEP).ToColumn("CEP", false);
            Map(x => x.DataInsercao).ToColumn("DATA_INSERCAO", false);
        }
    }
}