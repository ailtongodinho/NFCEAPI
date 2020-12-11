using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using NFCE.API.Interfaces;
using NFCE.API.Interfaces.Repositories;
using NFCE.API.Interfaces.Services;
using NFCE.API.Models;
using NFCE.API.Models.Request;
using NFCE.API.Models.Response.Item;

namespace NFCE.API.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;
        private readonly IProdutoService _produtoService;
        public ItemService(IItemRepository itemRepository, IProdutoService produtoService)
        {
            _itemRepository = itemRepository;
            _produtoService = produtoService;
        }
        #region CRUD
        /// <summary>
        /// Consultar
        /// </summary>
        /// <returns>Modelo de Consulta</returns>
        public ItemModel Consultar(int Id)
        {
            return _itemRepository.Consultar(Id);
        }
        /// <summary>
        /// Consultar pela nota
        /// </summary>
        /// <returns>Modelo de Consulta</returns>
        public IEnumerable<ItemModel> ConsultarPelaNota(int idNota)
        {
            return _itemRepository.GetList(x => x.IdControle == idNota).OrderBy(x => x.Nome);
        }
        /// <summary>
        /// Deletar
        /// </summary>
        /// <returns>Verdadeiro ou Falso</returns>
        public bool Deletar(int Id)
        {
            return _itemRepository.Delete(Id);
        }
        /// <summary>
        /// Novo Registro
        /// </summary>
        /// <param name="modelo">Modelo de Item</param>
        /// <returns>Identificação do Registro adicionado</returns>
        public int Novo(ItemModel modelo)
        {
            // modelo.IdProduto = _produtoService.Novo(new ProdutoModel(modelo));

            return _itemRepository.Insert(modelo);
        }
        #endregion
        #region Metodos
        /// <summary>
        /// Retorna agregado
        /// </summary>
        /// <param name="IdUsuario">Id do usuario</param>
        /// <param name="IdNota">Id da Nota/Controle</param>
        /// <returns>Agregado</returns>
        public ItemAgregadoResponse Agregado(int IdUsuario, int? IdNota = null)
        {
            return _itemRepository.Agregado(IdUsuario, IdNota);
        }
        #endregion
    }
}