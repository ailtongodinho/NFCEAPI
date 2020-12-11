using Microsoft.Extensions.Configuration;
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
using System.Threading.Tasks;
using NFCE.API.Interfaces.Services;
using NFCE.API.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;
using NFCE.API.Models.Handler;
using SimpleBrowser;
using NFCE.API.Models.Extracao;
using NFCE.API.Models.Response.Extracao;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Http;
using System.Text;

namespace NFCE.API.Services
{
    public class ExtracaoService : BaseService<NotaModel>, IExtracaoService
    {
        private readonly IConfiguration _config;
        private HtmlDocument document;
        private Browser browser;
        private ExtracaoRequest ExtracaoRequest;
        private ExtracaoResponse ExtracaoResponse;
        private readonly INotaService _NotaService;
        private readonly INotaRepository _NotaRepository;
        private readonly IEmissorService _EmissorService;
        private readonly IPagamentoService _PagamentoService;
        private readonly IItemService _ItemService;
        private static Dictionary<string, Browser> HttpHolder = new Dictionary<string, Browser>();
        private string BaseURL => _config.GetValue<string>("NFCE:BaseURL");
        private string CupomBaseURL => _config.GetValue<string>("Cupom:BaseUrl");
        private string CupomQuery => _config.GetValue<string>("Cupom:Query");
        private string CupomPattern => _config.GetValue<string>("Cupom:IdentificarPattern");
        private string CupomChavePattern => _config.GetValue<string>("Cupom:ChaveAcessoPattern");
        public ExtracaoService(
            INotaRepository NotaRepository,
            INotaService NotaService,
            IEmissorService EmissorService,
            IPagamentoService PagamentoService,
            IItemService ItemService
        )
        {
            _config = NotaRepository.GetConfiguration;
            _NotaRepository = NotaRepository;
            _NotaService = NotaService;
            _EmissorService = EmissorService;
            _PagamentoService = PagamentoService;
            _ItemService = ItemService;

            //  Browser
            browser = new Browser();
            // we'll fake the user agent for websites that alter their content for unrecognised browsers
            browser.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/534.10 (KHTML, like Gecko) Chrome/8.0.552.224 Safari/534.10";
        }
        /// <summary>
        /// Atribui o Tipo de Documento Fiscal
        /// </summary>
        /// <returns>Tipo de Documento Fiscal</returns>
        public ExtracaoTiposEnum SelecionaTipo()
        {
            ExtracaoTiposEnum tipo = ExtracaoTiposEnum.NotaFiscal;

            if (!ExtracaoRequest.Url.StartsWith(BaseURL))
            {
                //  Não é uma Nota Fiscal
                // if (Regex.Match(ExtracaoRequest.Url, @"\d+\|\d+\|").Success)
                if (ExtracaoRequest.Url.StartsWith(CupomBaseURL) || Regex.Match(ExtracaoRequest.Url, CupomPattern).Success)
                {
                    tipo = ExtracaoTiposEnum.CupomFiscal;
                }
            }

            return tipo;
        }
        /// <summary>
        /// Processa as informações da NFCE e salva no banco de dados
        /// </summary>
        /// <param name="idUsuario">Id do usuário</param>
        /// <param name="extracaoRequestModel">Parâmetros para iniciar a extração da NFCE</param>
        /// <returns>Resumo do processamento da NFCE</returns>
        public object Processar(int idUsuario, ExtracaoRequest extracaoRequestModel)
        {
            ExtracaoRequest = extracaoRequestModel;
            ExtracaoResponse = new ExtracaoResponse();
            string mensagem = _config.GetValue<string>("Messages:NFCE:Error");

            int tentativas = _config.GetValue<int>("Processamento:Tentativas");
            int contador = 0;
            var tipo = SelecionaTipo();
            NotaModel nota = null;

            while (contador < tentativas)
            {
                contador++;
                try
                {
                    switch (tipo)
                    {
                        case ExtracaoTiposEnum.NotaFiscal:
                            nota = ProcessarNotaFiscal();
                            break;
                        case ExtracaoTiposEnum.CupomFiscal:
                            // nota = ProcessarCupomFiscal();
                            nota = ProcessarCupomFiscalFisico(extracaoRequestModel.ExtracaoFisica);
                            break;
                    }
                    break;
                }
                catch (Exception ex)
                {
                    if (ex is HttpExceptionHandler) throw ex;
                    if (contador == tentativas)
                    {
                        throw new HttpExceptionHandler(mensagem, ex);
                    }
                }
            }
            nota.URL = extracaoRequestModel.Url;
            nota.Tentativas = contador;

            //  Inserindo no banco de dados
            if (nota.Valido)
            {
                ExtracaoResponse.Sucesso = true;
                nota.Status = StatusExtracaoEnum.Sucesso;
                mensagem = _config.GetValue<string>("Messages:NFCE:Success");
                //  Atribuindo ao usuário
                nota.IdUsuario = idUsuario;
                _NotaService.Novo(nota);
            }

            ExtracaoResponse.Mensagem = ExtracaoResponse.Mensagem ?? mensagem;
            ExtracaoResponse.Emissao = nota?.Emissao;
            ExtracaoResponse.IdNota = nota?.Id;

            return ExtracaoResponse;
        }
        public string Consultar(string chaveAcesso)
        {
            // browser.Navigate($"https://satsp.fazenda.sp.gov.br/COMSAT/Public/ConsultaPublica/ConsultaPublicaCfe.aspx");

            // if (LastRequestFailed(browser)) return null;

            // return browser.CurrentHtml;
            // var web = new HtmlWeb();
            // document = web.Load($"https://satsp.fazenda.sp.gov.br/COMSAT/Public/ConsultaPublica/ConsultaPublicaCfe.aspx");
            // // document = new HtmlDocument();
            // // document.Load() ();
            // return document.DocumentNode.InnerHtml;

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            // Pass the handler to httpclient(from you are calling api)
            HttpClient client = new HttpClient(clientHandler);

            var result = client.GetAsync($"https://satsp.fazenda.sp.gov.br/COMSAT/Public/ConsultaPublica/ConsultaPublicaCfe.aspx").Result;

            return result.Content.ReadAsStreamAsync().Result.ToString();
        }

        #region Cupom
        private void ConfigurarCupomFiscal()
        {
            //  Navega até o Site da Nota Fiscal
            // browser.Navigate(_config.GetValue<string>("Cupom:BaseAddress"));

            // if (LastRequestFailed(browser)) return;

            // var img = browser.Find("img", FindBy.Id, "conteudo_myImage1");

            // ExtracaoResponse.HostKey = Guid.NewGuid().ToString();

            // HttpHolder.Add(ExtracaoResponse.HostKey, browser);

            // ExtracaoResponse.Imagem = img.GetAttribute("src");
            // ExtracaoResponse.Mensagem = "Preencha os dados";

            // ....

            ExtracaoResponse.Url = $"{CupomBaseURL}{(string.Format(CupomQuery, Regex.Matches(ExtracaoRequest.Url, CupomChavePattern).FirstOrDefault().ToString()))}";
            ExtracaoResponse.Mensagem = "Por favor, preencha o Recaptcha";
        }
        private NotaModel ProcessarCupomFiscalFisico(ExtracaoAvancadoFisicoModel extracao)
        {
            NotaModel nota = new NotaModel();

            if (ExtracaoRequest.ExtracaoFisica == null)
            {
                ConfigurarCupomFiscal();
                return nota;
            }

            #region Nota

            document = new HtmlDocument();
            document.LoadHtml(Encoding.UTF8.GetString(Convert.FromBase64String(extracao.Nota)));

            nota = RetornaValorExtracao<NotaModel>(ExtracaoProcessamentoEnum.CupomFiscalAvancado, false).First();

            #endregion

            #region Emissor

            document = new HtmlDocument();
            document.LoadHtml(Encoding.UTF8.GetString(Convert.FromBase64String(extracao.Emissor)));

            nota.Emissor = RetornaValorExtracao<EmissorModel>(ExtracaoProcessamentoEnum.CupomFiscalAvancado).First();

            #endregion

            #region Pagamento

            document = new HtmlDocument();
            document.LoadHtml(Encoding.UTF8.GetString(Convert.FromBase64String(extracao.Pagamento)));

            nota.Pagamento = RetornaValorExtracao<PagamentoModel>(ExtracaoProcessamentoEnum.CupomFiscalAvancado).First();

            #endregion            

            #region Itens

            document = new HtmlDocument();
            document.LoadHtml(Encoding.UTF8.GetString(Convert.FromBase64String(extracao.Itens)));

            Dictionary<int, string> header = new Dictionary<int, string>();
            Dictionary<int, Dictionary<string, string>> itens_dict = new Dictionary<int, Dictionary<string, string>>();

            var select = new ExtracaoAttribute();
            select.HtmlTag = "table";
            select.HtmlId = "conteudo_grvProdutosServicos";
            foreach (HtmlNode table in document.DocumentNode.SelectNodes(select.FormataXPath(decendente: true)))
            {
                select = new ExtracaoAttribute();
                select.HtmlTag = "tr";
                int num_linha = 0;
                foreach (HtmlNode row in table.SelectNodes(select.FormataXPath(decendente: true)))
                {
                    if (header.Count == 0)
                    {
                        //  Seleciona o Header da Tabela
                        select.HtmlTag = "th";
                        var ths = row.SelectNodes(select.FormataXPath(decendente: true));
                        for (int i = 0; i < ths.Count; i++)
                        {
                            header.Add(i, FormataInnerText(ths[i].GetDirectInnerText()));
                        }
                    }
                    else
                    {
                        //  Seleciona todas as colunas
                        select.HtmlTag = "span";
                        select.AncestorTag = "td";
                        var itens = new Dictionary<string, string>();
                        var tds = row.SelectNodes(select.FormataXPath(decendente: true));
                        for (int i = 0; i < tds?.Count; i++)
                        {
                            itens.Add(header.GetValueOrDefault(i), FormataInnerText(tds[i].GetDirectInnerText()));
                        }
                        itens_dict.Add(num_linha, itens);
                        num_linha++;
                    }
                }
            }

            nota.Items = new List<ItemModel>();

            var tipoClasse = typeof(ItemModel);
            foreach (var dict in itens_dict)
            {
                ItemModel item = new ItemModel();
                foreach (var propriedade in tipoClasse.GetProperties())
                {
                    var atributo = tipoClasse.RetornaAtributos<ColumnAttribute>(propriedade.Name).FirstOrDefault();
                    if (atributo == null) continue;
                    var valor = dict.Value.GetValueOrDefault(atributo.Name);
                    //  Insere no item
                    propriedade.SetValue(item, Convert.ChangeType(valor, propriedade.PropertyType, System.Globalization.CultureInfo.GetCultureInfo("pt-BR")));
                }
                nota.Items.Add(item);
            }

            #endregion


            return nota;
        }
        private NotaModel ProcessarCupomFiscal()
        {
            NotaModel notaBasica = new NotaModel();

            if (string.IsNullOrEmpty(ExtracaoRequest.Imagem))
            {
                ConfigurarCupomFiscal();
            }
            else
            {
                notaBasica = ProcessarCupomFiscalAvancado();
            }

            return notaBasica;
        }
        private NotaModel ProcessarCupomFiscalAvancado()
        {
            NotaModel nota = new NotaModel();
            //  Navega até o Site da Nota Fiscal
            browser = HttpHolder.GetValueOrDefault(ExtracaoRequest.HostKey);
            //  Remove do Dicionario
            HttpHolder.Remove(ExtracaoRequest.HostKey);

            if (LastRequestFailed(browser)) return null;

            browser.Find("input", FindBy.Id, "conteudo_txtChaveAcesso").Value = ExtracaoRequest.Url.Split("|")[0];
            browser.Find("input", FindBy.Id, "conteudo_txtCaptcha").Value = ExtracaoRequest.Imagem;

            browser.Find(ElementType.Button, FindBy.Id, "conteudo_btnConsultar").Click();

            if (LastRequestFailed(browser)) return null;

            try
            {
                browser.Find(ElementType.Button, FindBy.Id, "conteudo_btnDetalhe").Click();
                document = new HtmlDocument();
                document.LoadHtml(browser.CurrentHtml);
            }
            catch (Exception ex)
            {
                throw new HttpExceptionHandler("O texto digitado não confere com o texto exibido na imagem. \n Tente novamente :)", ex);
            }

            #region Nota


            nota = RetornaValorExtracao<NotaModel>(ExtracaoProcessamentoEnum.CupomFiscalAvancado, false).First();


            #endregion

            #region Emissor

            //  Carregar tela de Emissor
            browser.Find("input", FindBy.Id, "conteudo_tabEmitente").Click();

            document = new HtmlDocument();
            document.LoadHtml(browser.CurrentHtml);

            nota.Emissor = RetornaValorExtracao<EmissorModel>(ExtracaoProcessamentoEnum.CupomFiscalAvancado).First();

            #endregion

            #region Pagamento

            //  Carregar tela de Pagamento
            browser.Find("input", FindBy.Id, "conteudo_tabTotais").Click();

            document = new HtmlDocument();
            document.LoadHtml(browser.CurrentHtml);

            nota.Pagamento = RetornaValorExtracao<PagamentoModel>(ExtracaoProcessamentoEnum.CupomFiscalAvancado).First();

            #endregion

            #region Itens

            //  Carregar tela de Itens
            browser.Find("input", FindBy.Id, "conteudo_tabProdutoServico").Click();

            document = new HtmlDocument();
            document.LoadHtml(browser.CurrentHtml);

            Dictionary<int, string> header = new Dictionary<int, string>();
            Dictionary<int, Dictionary<string, string>> itens_dict = new Dictionary<int, Dictionary<string, string>>();

            var select = new ExtracaoAttribute();
            select.HtmlTag = "table";
            select.HtmlId = "conteudo_grvProdutosServicos";
            foreach (HtmlNode table in document.DocumentNode.SelectNodes(select.FormataXPath(decendente: true)))
            {
                select = new ExtracaoAttribute();
                int num_linha = 0;
                foreach (HtmlNode row in table.SelectNodes("tr"))
                {
                    if (header.Count == 0)
                    {
                        //  Seleciona o Header da Tabela
                        select.HtmlTag = "th";
                        var ths = row.SelectNodes(select.FormataXPath(decendente: true));
                        for (int i = 0; i < ths.Count; i++)
                        {
                            header.Add(i, FormataInnerText(ths[i].GetDirectInnerText()));
                        }
                    }
                    else
                    {
                        //  Seleciona todas as colunas
                        select.HtmlTag = "span";
                        select.AncestorTag = "td";
                        var itens = new Dictionary<string, string>();
                        var tds = row.SelectNodes(select.FormataXPath(decendente: true));
                        for (int i = 0; i < tds?.Count; i++)
                        {
                            itens.Add(header.GetValueOrDefault(i), FormataInnerText(tds[i].GetDirectInnerText()));
                        }
                        itens_dict.Add(num_linha, itens);
                        num_linha++;
                    }
                }
            }

            nota.Items = new List<ItemModel>();

            var tipoClasse = typeof(ItemModel);
            foreach (var dict in itens_dict)
            {
                ItemModel item = new ItemModel();
                foreach (var propriedade in tipoClasse.GetProperties())
                {
                    var atributo = tipoClasse.RetornaAtributos<ColumnAttribute>(propriedade.Name).FirstOrDefault();
                    if (atributo == null) continue;
                    var valor = dict.Value.GetValueOrDefault(atributo.Name);
                    //  Insere no item
                    propriedade.SetValue(item, Convert.ChangeType(valor, propriedade.PropertyType, System.Globalization.CultureInfo.GetCultureInfo("pt-BR")));
                }
                nota.Items.Add(item);
            }

            #endregion

            return nota;
        }
        #endregion
        #region Nota Fiscal
        private List<string> ValidarNotaFiscal()
        {
            List<string> erros = new List<string>();
            //  Verifica URL
            if (!ExtracaoRequest.Url.StartsWith(BaseURL))
            {
                erros.Add($"Url não é Válida! Deve iniciar com {BaseURL}");
            }

            return erros;
        }
        private NotaModel ProcessarNotaFiscal()
        {
            var listaErros = ValidarNotaFiscal();

            if (listaErros.Count > 0) throw new HttpExceptionHandler(string.Join("\n", listaErros));
            //  Processamento Básico
            var notaBasica = ProcessarNotaFiscalBasico();
            //  Processamento Avançado
            var notaAvancada = ProcessarNotaFiscalAvancado();
            //  Emissão
            notaBasica.Emissao = notaAvancada.Emissao;
            //  Substitue itens para nota avançada
            notaBasica.Items = notaAvancada.Items;
            //  Emissor
            notaBasica.Emissor = notaAvancada.Emissor;
            //  Pagamento
            notaAvancada.Pagamento.TributosTotaisIncidentes = notaBasica.Pagamento.TributosTotaisIncidentes;
            notaAvancada.Pagamento.FormaPagamento = notaAvancada.Pagamento.FormaPagamento ?? notaBasica.Pagamento.FormaPagamento;
            notaBasica.Pagamento = notaAvancada.Pagamento;

            return notaBasica;
        }
        private NotaModel ProcessarNotaFiscalBasico()
        {
            NotaModel nota = new NotaModel();
            #region Ler HTML
            //  Navega até o Site da Nota Fiscal
            browser.Navigate(ExtracaoRequest.Url);

            if (LastRequestFailed(browser)) return null;

            document = new HtmlDocument();
            document.LoadHtml(browser.CurrentHtml);

            #endregion
            nota = RetornaValorExtracao<NotaModel>(ExtracaoProcessamentoEnum.NotaFiscalBasico).First();

            return nota;
        }
        private NotaModel ProcessarNotaFiscalAvancado()
        {
            NotaModel nota = new NotaModel();
            try
            {
                var btn_abas = browser.Find("input", FindBy.Id, "btnVisualizarAbas");

                btn_abas.Click();
                if (LastRequestFailed(browser)) return null;

                // HtmlResult aba = null;
                document = new HtmlDocument();
                document.LoadHtml(browser.CurrentHtml);

                #region Nota

                nota = RetornaValorExtracao<NotaModel>(ExtracaoProcessamentoEnum.NotaFiscalAvancado, false).FirstOrDefault();

                #endregion

                #region Emitente

                nota.Emissor = RetornaValorExtracao<EmissorModel>(ExtracaoProcessamentoEnum.NotaFiscalAvancado).FirstOrDefault();

                #endregion

                #region Produtos

                nota.Items = RetornaValorExtracao<ItemModel>(ExtracaoProcessamentoEnum.NotaFiscalAvancado);
                var itensAvancado = RetornaValorExtracao<ExtracaoAvancadoItemModel>(ExtracaoProcessamentoEnum.NotaFiscalAvancado);
                for (var i = 0; i < nota.Items.Count; i++)
                {
                    var item = nota.Items[i];
                    var itemAvancado = itensAvancado[i];
                    var novoItem = new ItemModel(item, itemAvancado);
                    nota.Items[i] = novoItem;
                }

                #endregion

                #region Pagamentos / Cobrança

                nota.Pagamento = RetornaValorExtracao<PagamentoModel>(ExtracaoProcessamentoEnum.NotaFiscalAvancado).FirstOrDefault();
                var pagamentoAvancado = RetornaValorExtracao<ExtracaoAvancadoPagamentoModel>(ExtracaoProcessamentoEnum.NotaFiscalAvancado).FirstOrDefault();
                nota.Pagamento = new PagamentoModel(nota.Pagamento, pagamentoAvancado);

                #endregion

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return nota;
        }
        #endregion
        public List<T> RetornaValorExtracao<T>(ExtracaoProcessamentoEnum tipoProcessamento, bool afundo = true) where T : new()
        {
            //  Pega a tipo da classe
            Type tipoClasse = typeof(T);
            //  Cria a lista da classe
            List<T> items = new List<T>();
            // IList items = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(tipoClasse));
            //  Pega o atributo da classe
            ExtracaoAttribute atributo = tipoClasse.RetornaAtributos<ExtracaoAttribute>().First(x => x.Tipo == tipoProcessamento);
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
                        if (!tipoPropriedade.IsSealed && afundo)
                        {
                            //  Verifica se é uma Lista
                            Type[] argumentos = tipoPropriedade.GetGenericArguments();
                            bool lista = argumentos.Count() > 0;
                            //  Se verdadeiro, atribui o Type para o 'tipoPropriedade'
                            if (lista) tipoPropriedade = argumentos[0];
                            //  Constroe o método
                            MethodInfo metodo = this.GetType().GetMethod("RetornaValorExtracao").MakeGenericMethod(new Type[] { tipoPropriedade });
                            //  Invoca o método para o Type
                            var retornoTipoPropriedade = metodo.Invoke(this, new object[] { tipoProcessamento, afundo });
                            //  Caso não seja uma lista, pega o primeiro item da mesma (Sempre retornamos uma lista)
                            if (!lista) retornoTipoPropriedade = ((IList)retornoTipoPropriedade)[0];
                            //  Insere no item
                            propriedade.SetValue(item, retornoTipoPropriedade);
                            //  Continua a iteração
                            continue;
                        }
                        //  Pega as informações do atributo
                        // var atributos = tipoClasse.RetornaAtributos<ExtracaoAttribute>(propriedade.Name);
                        atributo = tipoClasse.RetornaAtributos<ExtracaoAttribute>(propriedade.Name).FirstOrDefault(x => x.Tipo == tipoProcessamento);
                        // if (atributos.Count > 0)
                        // {
                        //     atributos = atributos.Where(x => x.Tipo == tipoProcessamento).ToList();
                        //     atributo = atributos.Count > 0 ? atributos.First() : null;
                        // }
                        //  Verifica atributo
                        if (atributo == null || atributo.Tipo == ExtracaoProcessamentoEnum.Default) continue;
                        //  Inicio o XPath
                        xpath = atributo.FormataXPath(decendente: true);
                        //  Pega o valor
                        string valor = node.SelectSingleNode(xpath)?.GetDirectInnerText();
                        //  Verifica o valor
                        if (string.IsNullOrEmpty(valor)) continue;
                        //  Aplica o Regex Padrão
                        valor = FormataInnerText(valor);
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
        static string FormataInnerText(string valor)
        {
            valor = new Regex(ExtracaoPatternsModel.RemoveNovaLinha).Replace(valor, string.Empty).Trim();
            valor = new Regex(ExtracaoPatternsModel.RemoveTab).Replace(valor, string.Empty).Trim();
            valor = new Regex(ExtracaoPatternsModel.RemoveCarrier).Replace(valor, string.Empty).Trim();

            return valor;
        }
        static bool LastRequestFailed(Browser browser)
        {
            if (browser.LastWebException != null)
            {
                browser.Log("There was an error loading the page: " + browser.LastWebException.Message);
                return true;
            }
            return false;
        }
    }
}