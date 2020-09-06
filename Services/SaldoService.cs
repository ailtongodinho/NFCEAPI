using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using NFCE.API.Interfaces.Repositories;
using NFCE.API.Interfaces.Services;
using NFCE.API.Models.Handler;
using NFCE.API.Models.Saldos;

namespace NFCE.API.Services
{
    public class SaldosService : ISaldosService
    {
        private readonly IConfiguration _config;
        private static ISaldosRepository _saldosRepository;
        public SaldosService(ISaldosRepository usuarioRepository)
        {
            _config = usuarioRepository.GetConfiguration;
            _saldosRepository = usuarioRepository;
        }
        public SaldosModel Consulta(int Id)
        {
            return _saldosRepository.GetById(Id);
        }
        public SaldosModel Consulta(SaldosModel model)
        {
            return _saldosRepository.GetList(x => x.IdProduto == model.IdProduto).FirstOrDefault();
        }
        public List<SaldosModel> Listar()
        {
            return _saldosRepository.GetAll().ToList();
        }
        public int Novo(SaldosModel model)
        {
            return _saldosRepository.Insert(model);
        }
        public bool Atualizar(SaldosModel model)
        {
            return _saldosRepository.Update(model);
        }
        /// <summary>
        /// Procedimento para Atualizar e adicionar novos saldos
        /// </summary>
        /// <returns>Sucesso do procedimento</returns>
        public bool AtualizarSaldos(int idNota)
        {
            return _saldosRepository.AtualizarSaldos(idNota);
        }
    }
}