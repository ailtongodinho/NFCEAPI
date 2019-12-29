using System.ComponentModel;

namespace NFCE.API.Enums
{
    public enum StatusExtracaoEnum
    {
        [Description("Extração realizada com sucesso")]
        Sucesso = 1,
        [Description("Erro ao extrair os dados")]
        Erro = -1
    }
}