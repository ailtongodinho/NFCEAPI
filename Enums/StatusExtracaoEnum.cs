using System.ComponentModel;

namespace NFCE.API.Enums
{
    public enum StatusExtracaoEnum
    {
        [Description("Nota registrada com sucesso!")]
        Sucesso = 1,
        [Description("Erro ao registrar a nota!")]
        Erro = -1
    }
}