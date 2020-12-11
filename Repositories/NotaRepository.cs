using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Extensions.Configuration;
using NFCE.API.Interfaces.Repositories;
using NFCE.API.Models;
using NFCE.API.Models.Handler;
using NFCE.API.Models.Request;
using NFCE.API.Models.Response.Nota;

namespace NFCE.API.Repositories
{
    public class NotaRepository : RepositoryBase<NotaModel>, INotaRepository
    {
        private readonly IConfiguration _config;
        private readonly IItemRepository _itemRepository;
        private readonly IRepositoryBase<EmissorModel> _emissorRepository;
        private readonly IRepositoryBase<PagamentoModel> _pagRepository;
        public NotaRepository(IConfiguration config,
            IItemRepository itemRepository,
            IRepositoryBase<EmissorModel> emissorRepository,
            IRepositoryBase<PagamentoModel> pagRepository) : base(config)
        {
            _config = config;
            _itemRepository = itemRepository;
            _emissorRepository = emissorRepository;
            _pagRepository = pagRepository;
        }
        public NotaModel Consultar(int Id)
        {
            //  Verifica se o item existe
            NotaModel nota = GetById(Id);
            if (nota == null) throw new HttpExceptionHandler("Item não existe");
            return Consultar(nota);
        }
        private NotaModel Consultar(NotaModel nota)
        {
            //  Adiciona o Emissor
            nota.Emissor = _emissorRepository.GetList(x => x.Id == nota.IdEmissor).FirstOrDefault();
            //  Adiciona Itens
            //  Quantidade de itens
            nota.QuantidadeItens = _itemRepository.ConsultaQuantidade(nota.Id);
            //  ValorTotal
            nota.ValorTotal = _itemRepository.ConsultaValorTotal(nota.Id);
            // nota.Items = _itemRepository.GetList(y => y.IdControle == nota.Id).ToList();
            //  Adiciona o Pagamento
            nota.Pagamento = _pagRepository.GetList(x => x.IdControle == nota.Id).FirstOrDefault();
            //  Retorna a nota completa
            return nota;
        }
        public int Novo(NotaModel modelo)
        {
            //  Insere na tabela de Controle
            return this.Insert(modelo);
        }
        public IEnumerable<NotaModel> Listar(int IdUsuario, NotaListarRequest extracaoListarRequest)
        {
            var modelo = new NotaModel
            {
                IdUsuario = IdUsuario,
                IdEmissor = extracaoListarRequest.IdEmissor ?? 0,
                Favorito = extracaoListarRequest.Favorito ?? false,
                Id = extracaoListarRequest.Top ?? 0,
                Emissao = extracaoListarRequest.DataFim ?? DateTime.Now
            };

            var where = new Dictionary<object, string>
            {
                { new { modelo.IdUsuario }, "="},
            };

            if (extracaoListarRequest.Favorito.HasValue)
            {
                where.Add(new { modelo.Favorito }, "=");
            }
            if (extracaoListarRequest.IdEmissor.HasValue)
            {
                where.Add(new { modelo.IdEmissor }, "=");
            }

            Dictionary<object, List<object>> between = null;
            if (!extracaoListarRequest.DataInicio.HasValue && extracaoListarRequest.DataFim.HasValue)
            {
                where.Add(new { modelo.Emissao }, "<=");
            }
            else if (extracaoListarRequest.DataInicio.HasValue && extracaoListarRequest.DataFim.HasValue)
            {
                between = new Dictionary<object, List<object>>{
                    { new { modelo.Emissao }, new List<object> { extracaoListarRequest.DataInicio.Value, extracaoListarRequest.DataFim.Value } }
                };
            }

            var listaExtracao = this.GetListCustom(
                modelo,
                where: where,
                between: between,
                top: extracaoListarRequest.Top
            ).ToList();

            listaExtracao.ForEach(x => x = Consultar(x));

            return listaExtracao;
        }
    }
}