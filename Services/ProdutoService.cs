using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using NFCE.API.Helpers;
using NFCE.API.Interfaces;
using NFCE.API.Interfaces.Repositories;
using NFCE.API.Interfaces.Services;
using NFCE.API.Models;
using NFCE.API.Models.Request;
using NFCE.API.Models.Response.Produto;

namespace NFCE.API.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoRepository _ProdutoRepository;
        public ProdutoService(IProdutoRepository ProdutoRepository)
        {
            _ProdutoRepository = ProdutoRepository;
        }
        #region CRUD
        /// <summary>
        /// Consultar
        /// </summary>
        /// <returns>Modelo de Consulta</returns>
        public ProdutoModel Consultar(int Id)
        {
            return _ProdutoRepository.GetById(Id);
        }
        /// <summary>
        /// Deletar
        /// </summary>
        /// <returns>Verdadeiro ou Falso</returns>
        public bool Deletar(int Id)
        {
            return _ProdutoRepository.Delete(Id);
        }
        /// <summary>
        /// Novo Registro
        /// </summary>
        /// <param name="modelo">Modelo de Produto</param>
        /// <returns>Identificação do Registro adicionado</returns>
        public int Novo(ProdutoModel modelo)
        {
            //  Retirando poeiras do nome
            modelo.Apelido = modelo.Nome.ToUpper().Replace(modelo.Unidade.ToUpper(), "");
            // Verifica se existe na lista
            var produto = Consultar(modelo);
            if (produto == null)
            {
                //  Aplica correção para codigos de identificação do produto
                modelo.CEST = long.TryParse(modelo.CEST, out long cest) ? cest.ToString() : null;
                modelo.CFOP = long.TryParse(modelo.CFOP, out long cfop) ? cfop.ToString() : null;
                modelo.EAN = long.TryParse(modelo.EAN, out long ean) ? ean.ToString() : null;
                modelo.NCM = long.TryParse(modelo.NCM, out long ncm) ? ncm.ToString() : null;
                modelo.DataCriacao = DateTime.Now;
                return _ProdutoRepository.Insert(modelo);
            }
            return produto.Id;
        }
        #endregion
        #region Metodos
        /// <summary>
        /// Listar
        /// </summary>
        /// <returns>Modelo de Listagem</returns>
        public IEnumerable<ProdutoResponse> Listar(ProdutoListarRequest produtoListarRequest)
        {
            if (!string.IsNullOrEmpty(produtoListarRequest.NomeProduto))
                produtoListarRequest.NomeProduto = produtoListarRequest.NomeProduto.RemoveAccents();
            return _ProdutoRepository.Listar(produtoListarRequest);
        }
        /// <summary>
        /// Consultar por produto
        /// </summary>
        /// <returns>Modelo de Consulta</returns>
        public ProdutoModel Consultar(ProdutoModel produto)
        {
            return _ProdutoRepository.GetList(x => x.Codigo == produto.Codigo).FirstOrDefault();
        }
        #endregion
    }
}