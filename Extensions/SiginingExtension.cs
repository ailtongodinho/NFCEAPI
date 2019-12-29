using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace NFCE.API.Extensions
{
    public class SingingExtension
    {
        public SecurityKey Chave { get; set; }
        public SigningCredentials Credenciais { get; set; }
        public SingingExtension()
        {
            // Cria uma chave com base em RSA com chave de tamanho 2048
            using(var provider = new RSACryptoServiceProvider(2048))
            {
                Chave = new RsaSecurityKey(provider.ExportParameters(true));
            }
            //  Cria a credêncial do usuário
            Credenciais  = new SigningCredentials(Chave, SecurityAlgorithms.RsaSha256Signature);
        }
    }
}