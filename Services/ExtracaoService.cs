using Microsoft.Extensions.Configuration;
using NFCE.API.Interfaces;
using NFCE.API.Attributes;
using NFCE.API.Models;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using NFCE.API.Extensions;
using System;
using System.Collections;
using System.Reflection;
using NFCE.API.Enums;
using System.ComponentModel;
using NFCE.API.Models.Request;
using NFCE.API.Models.Response;

namespace NFCE.API.Services
{
    public class ExtracaoService : IExtracaoService
    {
        private readonly IConfiguration config;
        private ExtracaoModel nfce;
        private HtmlDocument document;
        private readonly IExtracaoRepository _extracaoRepository;
        public ExtracaoService(IConfiguration _config, IExtracaoRepository extracaoRepository)
        {
            config = _config;
            _extracaoRepository = extracaoRepository;
        }
        /// <summary>
        /// Lista as notas ficais do usuário logado
        /// </summary>
        /// <returns>Lista de notas fiscais do usuário logado</returns>
        public ExtracaoListarResponse Listar(ExtracaoListarRequest extracaoListarRequest)
        {
            //  Filtrando pelo usuário atual da Sessão
            var listaExtracao = _extracaoRepository.Listar(AuthService.UsuarioAtual.Id, extracaoListarRequest);
            
            return new ExtracaoListarResponse {
                Dados = listaExtracao,
                Total = listaExtracao.Sum(x => x.Pagamento.ValorPago),
                Quantidade = listaExtracao.Count()
            };
        }
        /// <summary>
        /// Processa as informações da NFCE e salva no banco de dados
        /// </summary>
        /// <param name="extracaoRequestModel">Parâmetros para iniciar a extração da NFCE</param>
        /// <returns>REsumo do processamento da NFCE</returns>
        public BaseResponseModel ProcessarNFCE(ExtracaoRequestModel extracaoRequestModel)
        {
            bool sucesso = false;
            string message = "Erro ao processar a Nota Fiscal";
            try
            {

                nfce = new ExtracaoModel();
                #region Ler HTML

                document = new HtmlWeb().Load(extracaoRequestModel.Url);

                int tentativas = config.GetValue<int>("Processamento:Tentativas");
                int contador = 0;

                #endregion

                while (contador < tentativas)
                {
                    try
                    {
                        contador++;
                        nfce = RetornaValorExtracao<ExtracaoModel>().First();
                        if (nfce.Valido) break;
                    }
                    catch (Exception ex)
                    {
                        message = ex.Message;
                    }
                }

                nfce.URL = extracaoRequestModel.Url;
                nfce.Tentativas = contador;
                sucesso = nfce.Valido;

                //  Inserindo no banco de dados
                if (sucesso)
                {
                    nfce.Status = StatusExtracaoEnum.Sucesso;
                    //  Atribuindo ao usuário
                    nfce.IdUsuario = AuthService.UsuarioAtual.Id;
                    _extracaoRepository.Salvar(nfce);
                }

                message = typeof(StatusExtracaoEnum).RetornaAtributo<DescriptionAttribute>(nfce.Status.ToString()).Description;
            }
            catch (Exception ex)
            {
                sucesso = false;
                message = ex.InnerException?.Message ?? ex.Message;
            }
            finally
            {
                //  Log
                
            }

            return new BaseResponseModel { Sucesso = sucesso, Mensagem = message };
        }

        public List<T> RetornaValorExtracao<T>() where T : new()
        {
            //  Pega a tipo da classe
            Type tipoClasse = typeof(T);
            //  Cria a lista da classe
            List<T> items = new List<T>();
            // IList items = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(tipoClasse));
            //  Pega o atributo da classe
            ExtracaoAttribute atributo = tipoClasse.RetornaAtributo<ExtracaoAttribute>();
            //  Processa o XPath
            string xpath = atributo.FormataXPath();
            //  Procura pela Tag do XPath
            var nodes = document.DocumentNode.SelectNodes(xpath);
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    //  Inicializa o item
                    T item = new T();
                    foreach (var propriedade in tipoClasse.GetProperties())
                    {
                        //  Pega o tipo da propriedade
                        Type tipoPropriedade = propriedade.PropertyType;
                        if (!tipoPropriedade.IsSealed)
                        {
                            //  Verifica se é uma Lista
                            Type[] argumentos = tipoPropriedade.GetGenericArguments();
                            bool lista = argumentos.Count() > 0;
                            //  Se verdadeiro, atribui o Type para o 'tipoPropriedade'
                            if (lista) tipoPropriedade = argumentos[0];
                            //  Constroe o método
                            MethodInfo metodo = this.GetType().GetMethod("RetornaValorExtracao").MakeGenericMethod(new Type[] { tipoPropriedade });
                            //  Invoca o método para o Type
                            var retornoTipoPropriedade = metodo.Invoke(this, new object[] { });
                            //  Caso não seja uma lista, pega o primeiro item da mesma (Sempre retornamos uma lista)
                            if (!lista) retornoTipoPropriedade = ((IList)retornoTipoPropriedade)[0];
                            //  Insere no item
                            propriedade.SetValue(item, retornoTipoPropriedade);
                            //  Continua a iteração
                            continue;
                        }
                        //  Pega as informações do atributo
                        atributo = tipoClasse.RetornaAtributo<ExtracaoAttribute>(propriedade.Name);
                        //  Verifica atributo
                        if (atributo == null) continue;
                        //  Inicio o XPath
                        xpath = atributo.FormataXPath(decendente: true);
                        //  Pega o valor
                        string valor = node.SelectSingleNode(xpath)?.GetDirectInnerText();
                        //  Verifica o valor
                        if (string.IsNullOrEmpty(valor)) continue;
                        //  Aplica o Regex Padrão
                        valor = new Regex(ExtracaoPatternsModel.RemoveNovaLinha).Replace(valor, string.Empty).Trim();
                        valor = new Regex(ExtracaoPatternsModel.RemoveTab).Replace(valor, string.Empty).Trim();
                        valor = new Regex(ExtracaoPatternsModel.RemoveCarrier).Replace(valor, string.Empty).Trim();
                        //  Aplica o Regex
                        if (atributo.Pattern != null)
                        {
                            foreach (var pattern in atributo.Pattern.Split(ExtracaoPatternsModel.Delimiter))
                            {
                                var join = Regex.Matches(valor, pattern).ToList();
                                if (atributo.RegexIndex > 0) join = join.Where(x => x == join[atributo.RegexIndex - 1]).ToList();
                                valor = string.Join(string.Empty, join);
                                valor = valor.Trim();
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(valor))
                        {
                            //  Insere no item
                            propriedade.SetValue(item, Convert.ChangeType(valor, tipoPropriedade, System.Globalization.CultureInfo.GetCultureInfo("pt-BR")));
                        }
                    }
                    //  Adiciona na lista
                    items.Add(item);
                }
            }
            return items;
        }
    }
}