using System.Collections.Generic;
using System.Linq;
using NFCE.API.Models.Response;

namespace NFCE.API.Models
{
    public enum OrdernarEnum
    {
        data_asc = 0,
        data_desc = 1,
        preco_asc = 2,
        preco_desc = 3,
        lojas_asc = 4,
        lojas_desc = 5,
    }
    public class ConfiguracaoModel
    {
        #region Sexo
        public List<PickerResponse> Opcoes = new List<PickerResponse> {
            new PickerResponse("Masculino", "M", "Masculino"),
            new PickerResponse("Feminino", "F", "Feminino"),
            new PickerResponse("Outro", "O", "Outro")
        };
        public PickerResponse GetSexoPorChave(string chave)
        {
            return Opcoes.Where(x => x.Id == chave).First();
        }
        #endregion
        #region Ordenar
        public List<PickerResponse> Ordenar = new List<PickerResponse> {
            new PickerResponse("Data (Mais antigo)", OrdernarEnum.data_asc),
            new PickerResponse("Data (Mais recente)", OrdernarEnum.data_desc),
            new PickerResponse("Preço (crescente)", OrdernarEnum.preco_asc),
            new PickerResponse("Preço (decrescente)", OrdernarEnum.preco_desc),
            new PickerResponse("Lojas (A a Z)", OrdernarEnum.lojas_asc),
            new PickerResponse("Lojas (Z a A)", OrdernarEnum.lojas_desc),
        };
        public PickerResponse GetOrdenarPorChave(string chave)
        {
            if (string.IsNullOrEmpty(chave)) return Ordenar.First();
            return Ordenar.Where(x => x.Id == chave).First();
        }
        public void OrdernarLista(ref List<NotaModel> lista, string chave)
        {
            //  Ordenando Dados
            var ordernar = GetOrdenarPorChave(chave);

            IEnumerable<NotaModel> _lista = null;

            switch ((OrdernarEnum)ordernar.Valor)
            {
                case OrdernarEnum.data_asc:
                    _lista = lista.OrderBy(x => x.Emissao);
                    break;
                case OrdernarEnum.data_desc:
                    _lista = lista.OrderByDescending(x => x.Emissao);
                    break;
                case OrdernarEnum.preco_asc:
                    _lista = lista.OrderBy(x => x.ValorTotal);
                    break;
                case OrdernarEnum.preco_desc:
                    _lista = lista.OrderByDescending(x => x.ValorTotal);
                    break;
                case OrdernarEnum.lojas_asc:
                    _lista = lista.OrderBy(x => x.Emissor.RazaoSocial);
                    break;
                case OrdernarEnum.lojas_desc:
                    _lista = lista.OrderByDescending(x => x.Emissor.RazaoSocial);
                    break;
            }

            if (_lista != null) lista = _lista.ToList();
        }
        #endregion
    }
}