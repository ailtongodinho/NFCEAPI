using Dapper.FluentMap.Dommel.Mapping;
using NFCE.API.Models;

namespace NFCE.API.Mapping
{
    public class AuthMapping : DommelEntityMap<AuthModel>
    {
        public AuthMapping(){
            ToTable("NFCET_LOGIN");
            Map(x => x.Id).ToColumn("ID", true).IsIdentity();
            Map(x => x.IdUsuario).ToColumn("ID_USR", true);
            Map(x => x.Senha).ToColumn("SENHA", true);
            Map(x => x.DataUltimo).ToColumn("DATA_ULTIMO", true);
            Map(x => x.Login).Ignore();
            Map(x => x.Valido).Ignore();
        }
    }
}