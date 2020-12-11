using Dapper.FluentMap.Dommel.Mapping;
using NFCE.API.Models;

namespace NFCE.API.Mapping
{
    public class LocalidadeMapping : DommelEntityMap<LocalidadeModel>
    {
        public LocalidadeMapping()
        {
            ToTable("NFCET_LOCALIDADE");
            Map(x => x.Id).ToColumn("ID", false).IsIdentity();
            Map(x => x.RegiaoId).ToColumn("REGIAO_ID", false);
            Map(x => x.RegiaoSigla).ToColumn("REGIAO_SIGLA", false);
            Map(x => x.RegiaoNome).ToColumn("REGIAO_NOME", false);
            Map(x => x.UFId).ToColumn("UF_ID", false);
            Map(x => x.UFSigla).ToColumn("UF_SIGLA", false);
            Map(x => x.UFNome).ToColumn("UF_NOME", false);
            Map(x => x.MesorregiaoId).ToColumn("MESORREGIAO_ID", false);
            Map(x => x.MesorregiaoNome).ToColumn("MESORREGIAO_NOME", false);
            Map(x => x.MicrorregiaoId).ToColumn("MICROREGIAO_ID", false);
            Map(x => x.MicrorregiaoNome).ToColumn("MICROREGIAO_NOME", false);
            Map(x => x.MunicipioId).ToColumn("MUNICIPIO_ID", false);
            Map(x => x.MunicipioNome).ToColumn("MUNICIPIO_NOME", false);
            Map(x => x.DistritoId).ToColumn("DISTRITO_ID", false);
            Map(x => x.DistritoNome).ToColumn("DISTRITO_NOME", false);
        }
    }
}