using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using NFCE.API.Interfaces;
using NFCE.API.Interfaces.Repositories;
using NFCE.API.Interfaces.Services;
using NFCE.API.Models;
using NFCE.API.Models.Handler;
using NFCE.API.Models.Request;
using NFCE.API.Models.Response.Nota;
using NFCE.API.Models.Saldos;

namespace NFCE.API.Services
{
    public class NotaService : BaseService<NotaModel>, INotaService
    {
        private readonly INotaRepository _NotaRepository;
        private readonly IItemService _ItemService;
        private readonly IPagamentoService _PagamentoService;
        private readonly IEmissorService _EmissorService;
        private readonly ISaldosService _saldosService;
        private readonly ConfiguracaoModel _ConfiguracaoModel;
        private readonly IProdutoService _ProdutoService;
        private readonly IConfiguration _config;
        public NotaService(
            INotaRepository NotaRepository,
            IItemService ItemService,
            IPagamentoService PagamentoService,
            IEmissorService EmissorService,
            ISaldosService saldosService,
            ConfiguracaoModel ConfiguracaoModel,
            IProdutoService produtoService
        )
        // : base(httpContextAccessor, usuarioService)
        {
            _NotaRepository = NotaRepository;
            _ItemService = ItemService;
            _PagamentoService = PagamentoService;
            _EmissorService = EmissorService;
            _ConfiguracaoModel = ConfiguracaoModel;
            _saldosService = saldosService;
            _config = _NotaRepository.GetConfiguration;
            _ProdutoService = produtoService;
        }
        #region CRUD
        /// <summary>
        /// Consultar
        /// </summary>
        /// <returns>Modelo de Consulta</returns>
        public NotaModel Consultar(int Id)
        {
            var lista = _NotaRepository.Consultar(Id);
            lista.Items = _ItemService.ConsultarPelaNota(Id).ToList();
            return lista;
        }
        /// <summary>
        /// Deletar
        /// </summary>
        /// <returns>Verdadeiro ou Falso</returns>
        public object Deletar(int Id)
        {
            _NotaRepository.OpenTransaction();
            try
            {
                var modelo = Consultar(Id);

                //  Deleta da tabela de Items
                modelo.Items.ForEach(x =>
                {
                    _ItemService.Deletar(x.Id);
                });
                //  Deleta da tabela de Pagamentos
                if (modelo.Pagamento != null)
                    _PagamentoService.Deletar(modelo.Pagamento.Id);
                //  Deleta da tabela de Emissor
                // if (modelo.Emissor != null)
                //     _EmissorService.Deletar(modelo.Emissor.Id);
                //  Deletar a Nota
                _NotaRepository.Delete(modelo.Id);
                _NotaRepository.CloseTransaction(true);
                return new
                {
                    Id = modelo.Id,
                    Mensagem = "Nota deletada com sucesso!"
                };
            }
            catch (Exception ex)
            {
                //  Rollback
                _NotaRepository.CloseTransaction(false);
                throw ex;
            }
        }
        /// <summary>
        /// Salvar uma nova Nota
        /// </summary>
        /// <param name="modelo">Modelo completo de uma nota</param>
        public int Novo(NotaModel modelo)
        {
            _NotaRepository.OpenTransaction();
            try
            {
                if (Existe(modelo)) throw new HttpExceptionHandler(mensagemUsuario: _config.GetValue<string>("Messages:NFCE:Exists"));
                //  Insere na tabela de Emissor
                modelo.IdEmissor = _EmissorService.Novo(modelo.Emissor);
                //  Pega o Id
                modelo.Id = _NotaRepository.Novo(modelo);
                //  Atribui as propriedades
                modelo.Pagamento.IdControle = modelo.Id;
                //  Insere na tabela de Items
                Task.Run(() =>
                {
                    modelo.Items.ForEach(x =>
                    {
                        x.IdControle = modelo.Id;
                        x.IdProduto = _ProdutoService.Novo(new ProdutoModel(x)
                        {
                            IdEmissor = modelo.IdEmissor
                        });
                        _ItemService.Novo(x);
                        _saldosService.Salvar(x, modelo.IdEmissor);
                    });
                }).ContinueWith((x) => { x.Dispose(); }, TaskContinuationOptions.OnlyOnRanToCompletion);
                //  Insere na tabela de Pagamentos
                _PagamentoService.Novo(modelo.Pagamento);
                //  Insere na Saldos
                // _saldosService.AtualizarSaldos(modelo.Id);
                _NotaRepository.CloseTransaction(true);
            }
            catch (Exception ex)
            {
                //  Rollback
                _NotaRepository.CloseTransaction(false);
                throw ex;
            }

            return modelo.Id;
        }
        #endregion
        #region Metodos
        /// <summary>
        /// Favorita um item da lista
        /// </summary>
        /// <returns>Verdadeiro ou falso</returns>
        public bool Favoritar(NotaRequest notaRequest)
        {
            var nota = Consultar(notaRequest.Id);
            //  Muda o favorito
            nota.Favorito = !nota.Favorito;
            //  Atualiza o campo de Favorito
            return _NotaRepository.UpdateGenerico(nota, new { nota.Favorito });
        }
        /// <summary>
        /// Lista as notas ficais do usuário logado
        /// </summary>
        /// <returns>Lista de notas fiscais do usuário logado</returns>
        // public IEnumerable<NotaListarResponse> ListarTotais(int idUsuario, NotaListarRequest extracaoListarRequest)
        public NotaListarAgregadoResponse ListarTotais(int idUsuario, NotaListarRequest extracaoListarRequest)
        {
            //  Filtrando pelo usuário atual da Sessão
            var listaExtracao = new NotaOrdenarRequest
            {
                Lista = _NotaRepository.Listar(idUsuario, extracaoListarRequest).ToList(),
                OrdernarId = extracaoListarRequest.OrdernarId
            };

            // if(listaExtracao.Lista.Count == 0) throw new HttpExceptionHandler("Não há mais dados");

            var retorno = new NotaListarAgregadoResponse(Ordenar(listaExtracao));

            retorno.Ultimo = retorno.Lista.Count() < (extracaoListarRequest.Top ?? retorno.Lista.Count());

            return retorno;
        }
        /// <summary>
        /// Ordena as notas ficais
        /// </summary>
        /// <returns>Lista de notas fiscais Ordenada</returns>
        // public NotaListarResponse Ordenar(NotaOrdenarRequest notaOrdenarRequest)
        public IEnumerable<NotaListarResponse> Ordenar(NotaOrdenarRequest notaOrdenarRequest)
        {
            var novaLista = notaOrdenarRequest.Lista;

            if (novaLista == null)
            {
                novaLista = new List<NotaModel>();
                foreach (var item in notaOrdenarRequest.ListaAgrupada.Select(x => x.Dados))
                {
                    novaLista.AddRange(item);
                }
            }

            //  Ordena
            _ConfiguracaoModel.OrdernarLista(ref novaLista, notaOrdenarRequest.OrdernarId);

            // return new NotaListarResponse(novaLista);
            return novaLista
                .GroupBy(x => x.Emissao.Date)
                .Select(x => new NotaListarResponse(x.Key, x.Select(y => y)))
                .OrderByDescending(x => x.Data);
        }
        /// <summary>
        /// Listar agregado de notas
        /// </summary>
        /// <returns>Agregado de todas as notas</returns>
        public NotaAgregadoResponse Agregado()
        {
            var agregado = new NotaAgregadoResponse();
            agregado.ItemAgregado = _ItemService.Agregado(Usuario.Id);
            agregado.PagamentoAgregado = _PagamentoService.Agregado(Usuario.Id);
            return agregado;
        }
        public bool Existe(NotaModel modelo)
        {
            //  Verifica se existe
            return _NotaRepository.GetList(x => x.ChaveAcesso == modelo.ChaveAcesso && x.IdUsuario == modelo.IdUsuario).Any();
        }
        public bool Existe(string chaveAcesso)
        {
            //  Verifica se existe
            return _NotaRepository.GetList(x => x.ChaveAcesso == chaveAcesso).Any();
        }
        #endregion
    }
}