using System.Collections.Generic;
using NFCE.API.Models;
using NFCE.API.Models.Saldos;

namespace NFCE.API.Interfaces.Services
{
    public interface ISaldosService
    {
        int Novo(SaldosModel model);
        SaldosModel Consulta(int Id);
        SaldosModel Consulta(SaldosModel model);
        List<SaldosModel> Listar();
        bool Atualizar(SaldosModel model);
        bool AtualizarSaldos(int idNota);
    }
}