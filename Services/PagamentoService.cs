using NFCE.API.Interfaces.Repositories;
using NFCE.API.Interfaces.Services;
using NFCE.API.Models;
using NFCE.API.Models.Response.Pagamento;

namespace NFCE.API.Services
{
    public class PagamentoService : IPagamentoService
    {
        private readonly IPagamentoRepository _PagamentoRepository;
        public PagamentoService(IPagamentoRepository PagamentoRepository)
        {
            _PagamentoRepository = PagamentoRepository;
        }
        #region CRUD
        /// <summary>
        /// Consultar
        /// </summary>
        /// <returns>Modelo de Consulta</returns>
        public PagamentoModel Consultar(int Id)
        {
            return _PagamentoRepository.Consultar(Id);
        }
        /// <summary>
        /// Deletar
        /// </summary>
        /// <returns>Verdadeiro ou Falso</returns>
        public bool Deletar(int Id)
        {
            return _PagamentoRepository.Delete(Id);
        }
        /// <summary>
        /// Novo Registro
        /// </summary>
        /// <param name="modelo">Modelo de Pagamento</param>
        /// <returns>Identificação do Registro adicionado</returns>
        public int Novo(PagamentoModel modelo)
        {
            return _PagamentoRepository.Insert(modelo);
        }
        #endregion
        #region Metodos
        /// <summary>
        /// Retorna agregado
        /// </summary>
        /// <param name="IdUsuario">Id do usuario</param>
        /// <param name="IdNota">Id da Nota/Controle</param>
        /// <returns>Agregado</returns>
        public PagamentoAgregadoResponse Agregado(int IdUsuario, int? IdNota = null)
        {
            return _PagamentoRepository.Agregado(IdUsuario, IdNota);
        }
        #endregion
    }
}