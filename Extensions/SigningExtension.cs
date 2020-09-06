using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace NFCE.API.Extensions
{
    public class SigningExtension
    {
        public SymmetricSecurityKey Chave { get; set; }
        // public SecurityKey Chave { get; set; }
        public static string Segredo = "fedaf7d8863b48e197b9287d492b708e";
        public SigningCredentials Credenciais { get; set; }
        public SigningExtension()
        {
            // Cria uma chave com base em RSA com chave de tamanho 2048
            // using (var provider = new RSACryptoServiceProvider(2048))
            // {
            //     Chave = new RsaSecurityKey(provider.ExportParameters(true));
            // }
            Chave = new SymmetricSecurityKey(Encoding.Default.GetBytes(Segredo));
            //  Cria a credêncial do usuário
            // Credenciais = new SigningCredentials(Chave, SecurityAlgorithms.RsaSha256Signature);
            Credenciais = new SigningCredentials(Chave, SecurityAlgorithms.HmacSha256Signature);
        }
    }
}