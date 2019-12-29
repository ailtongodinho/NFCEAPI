using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using NFCE.API.Interfaces;
using NFCE.API.Models;

namespace NFCE.API.Repositories
{
    public class ExtracaoRepository : BaseRepository<ExtracaoModel>, IExtracaoRepository
    {
        private readonly IConfiguration _config;
        private readonly IBaseRepository<ExtracaoItemModel> _itemRepository;
        private readonly IBaseRepository<ExtracaoEmissorModel> _emissorRepository;
        private readonly IBaseRepository<ExtracaoPagamentoModel> _pagRepository;
        public ExtracaoRepository(IConfiguration config,
            IBaseRepository<ExtracaoItemModel> itemRepository,
            IBaseRepository<ExtracaoEmissorModel> emissorRepository,
            IBaseRepository<ExtracaoPagamentoModel> pagRepository) : base(config)
        {
            _config = config;
            _itemRepository = itemRepository;
            _emissorRepository = emissorRepository;
            _pagRepository = pagRepository;
        }
        public int Novo(ExtracaoModel model)
        {
            //  Verifica se existe
            var existe = this.GetList(x => x.ChaveAcesso == model.ChaveAcesso).Any();
            //  Se existir, retorna falso
            if (existe) throw new Exception(_config.GetValue<string>("Messages:Error:NFCE:Exists"));
            //  Insere na tabela de Controle
            model.Id = this.Insert(model);
            //  Retorna Id
            return model.Id;
        }
        public void Salvar(ExtracaoModel model)
        {
            //  Pega o Id
            Novo(model);
            //  Atribui as propriedades
            model.Emissor.IdControle = model.Id;
            model.Pagamento.IdControle = model.Id;
            //  Insere na tabela de Items
            model.Items.ForEach(x =>
            {
                x.IdControle = model.Id;
                _itemRepository.Insert(x);
            });
            //  Insere na tabela de Pagamentos
            _pagRepository.Insert(model.Pagamento);
            //  Insere na tabela de Emissor
            _emissorRepository.Insert(model.Emissor);
        }
    }
}