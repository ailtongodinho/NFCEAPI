using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using NFCE.API.Interfaces;
using NFCE.API.Interfaces.Repositories;
using NFCE.API.Interfaces.Services;
using NFCE.API.Models;
using NFCE.API.Models.Handler;
using NFCE.API.Models.Request;

namespace NFCE.API.Services
{
    public class ComprasService : IComprasService
    {
        private readonly IComprasRepository _ComprasRepository;
        private readonly IComprasProdutoService _ComprasProdutoService;
        public ComprasService(IComprasRepository ComprasRepository, IComprasProdutoService comprasProdutoService)
        {
            _ComprasRepository = ComprasRepository;
            _ComprasProdutoService = comprasProdutoService;
        }
        #region CRUD
        /// <summary>
        /// Consultar
        /// </summary>
        /// <returns>Modelo de Consulta</returns>
        public ComprasModel Consultar(int Id)
        {
            return _ComprasRepository.GetById(Id);
        }
        /// <summary>
        /// Deletar
        /// </summary>
        /// <returns>Verdadeiro ou Falso</returns>
        public bool Deletar(int Id)
        {
            _ComprasProdutoService.DeletarPorCompra(Id);
            return _ComprasRepository.Delete(Id);
        }
        /// <summary>
        /// Novo Registro
        /// </summary>
        /// <param name="modelo">Modelo de Compras</param>
        /// <returns>Identificação do Registro adicionado</returns>
        public int Novo(ComprasModel modelo)
        {
            modelo.DataCriacao = DateTime.Now;
            modelo.Total = 0;
            if (string.IsNullOrEmpty(modelo.Nome)) throw new HttpExceptionHandler("Nome da lista deve ser preenchido");
            if (modelo.IdLocalidade == 0) throw new HttpExceptionHandler("Seleciona o municipio ou distrito");

            return _ComprasRepository.Insert(modelo);
        }
        #endregion
        #region Metodos
        /// <summary>
        /// Listar
        /// </summary>
        /// <returns>Modelo de Listagem</returns>
        public IEnumerable<ComprasModel> Listar(int idUsuario)
        {
            return _ComprasRepository.GetList(x => x.IdUsuario == idUsuario).OrderByDescending(x => x.DataCriacao);
        }
        #endregion
    }
}