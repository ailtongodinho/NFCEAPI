using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using NFCE.API.Interfaces.Repositories;
using NFCE.API.Interfaces.Services;
using NFCE.API.Models;
using NFCE.API.Models.Handler;
using NFCE.API.Models.Saldos;

namespace NFCE.API.Services
{
    public class SaldosService : ISaldosService
    {
        private readonly IConfiguration _config;
        private static ISaldosRepository _saldosRepository;
        public SaldosService(ISaldosRepository saldosRepository)
        {
            _config = saldosRepository.GetConfiguration;
            _saldosRepository = saldosRepository;
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
        /// Procedimento para Salvar saldos
        /// </summary>
        /// <returns>Sucesso do procedimento</returns>
        public bool Salvar(ItemModel item, int idEmissor)
        {
            bool retorno = true;
            SaldosModel saldo = new SaldosModel()
            {
                IdProduto = item.IdProduto,
                IdEmissor = idEmissor,
                ValorUnitario = item.ValorUnitario
            };
            var saldoDB = _saldosRepository.GetList(x => x.IdProduto == item.IdProduto && x.IdEmissor == idEmissor && x.Ultimo == 1).FirstOrDefault();
            if (saldoDB != null)
            {
                //  Deletar itens do mesmo emissor com o valor igual
                if (saldoDB.ValorUnitario != item.ValorUnitario)
                {
                    //  Atualiza o campo "Ultimo"
                    saldoDB.Ultimo = 0;
                    _saldosRepository.Update(saldoDB);
                }
                else
                {
                    retorno = false;
                }
            }
            //  Insere na base
            saldo.Ultimo = 1;
            saldo.DataInsercao = DateTime.Now;
            _saldosRepository.Insert(saldo);

            return retorno;
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