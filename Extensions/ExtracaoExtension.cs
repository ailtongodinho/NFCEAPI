using System.Collections.Generic;
using NFCE.API.Attributes;

namespace NFCE.API.Extensions
{
    public static class ExtracaoExtension
    {
        private const string atributoClasse = "@class";
        private const string atributoId = "@id";
        private const string atributoTexto = "text()";
        private const string atributoNome = "name()";
        public static string FormataXPath(this ExtracaoAttribute atributo, bool decendente = false)
        {
            List<string> XPath = new List<string>();
            #region Parâmetros
            if (decendente) XPath.Add(".");
            XPath.Add(atributo.FormataXPathBase());
            #endregion
            #region Following Parâmetros
            XPath.Add(atributo.FormataXPathFollowing());
            #endregion
            #region Preceding Parâmetros
            XPath.Add(atributo.FormataXPathPreceding());
            #endregion
            //  Fecha o XPath
            XPath.Add("]");
            #region Index Parâmetros
            if (atributo.Index > 0)
            {
                XPath.Insert(0, "(");
                XPath.Add($")[{atributo.Index}]");
            }
            else
            {
                for (int i = 0; i > atributo.Index; i--)
                {
                    XPath.Add(@"/..");
                }
            }
            #endregion
            return string.Join("", XPath);
        }
        /// <summary>
        /// Retorna em String uma condição apra XPath
        /// </summary>
        /// <param name="condicao">True = "And" | False = "Or"</param>
        /// <returns></returns>
        private static string RetornaCondicao(bool? condicao = null)
        {
            string retorno = string.Empty;
            if (condicao != null)
                retorno = $" {(condicao.Value ? "and" : "or")} ";
            return retorno;
        }
        /// <summary>
        /// Retorna em String a sintax de "Contains"
        /// </summary>
        /// <param name="atributo"></param>
        /// <param name="valor"></param>
        /// <param name="condicao">Condição And ou Or</param>
        /// <returns></returns>
        private static string RetornaFuncionalidade(string atributo, string valor, bool? condicao = null)
        {
            string retorno = string.Empty;
            if (!string.IsNullOrEmpty(valor))
                retorno = $"{RetornaCondicao(condicao)}contains({atributo}, '{valor}')";
            return retorno;
        }
        /// <summary>
        /// Retorna em String a sintax de "Following"
        /// </summary>
        /// <param name="tipo">Tipo de funcionalidade</param>
        /// <param name="tag">Tag a ser aplicada</param>
        /// <param name="filtro">Caso não seja nulo, é aplicado um filtro</param>
        /// /// <param name="condicao">Condição And ou Or</param>
        /// <returns></returns>
        private static string RetornaFuncionalidade(string tipo, string tag, string filtro, bool? condicao = null)
        {
            string retorno = string.Empty;
            if (!string.IsNullOrEmpty(tag))
                retorno = $"{RetornaCondicao(condicao)}{tipo}::{tag}[{RetornaFuncionalidade(atributoNome, tag)}{(filtro ?? string.Empty)}]";
            return retorno;
        }
        private static string FormataXPathBase(this ExtracaoAttribute atributo)
        {
            string XPath = $"//{atributo.HtmlTag}[";
            //  Tag principal
            XPath += RetornaFuncionalidade(atributoNome, atributo.HtmlTag);
            //  Adiciona Classe
            XPath += RetornaFuncionalidade(atributoClasse, atributo.HtmlClass, condicao: true);
            //  Adiciona Id
            XPath += RetornaFuncionalidade(atributoId, atributo.HtmlId, condicao: true);
            //  Adiciona Texto
            if (!string.IsNullOrWhiteSpace(atributo.HtmlText))
                XPath += RetornaFuncionalidade(atributoTexto, atributo.HtmlText, condicao: true);
            return XPath;
        }
        private static string FormataXPathFollowing(this ExtracaoAttribute atributo)
        {
            string XPath = string.Empty;
            if (!string.IsNullOrEmpty(atributo.FollowingTag))
            {
                string funcionalidade = RetornaFuncionalidade(atributoClasse, atributo.FollowingClass, condicao: true);
                funcionalidade += RetornaFuncionalidade(atributoId, atributo.FollowingId, condicao: true);
                funcionalidade += RetornaFuncionalidade(atributoTexto, atributo.FollowingText, condicao: true);
                //  Tag Following
                XPath = $"{RetornaCondicao(true)}{RetornaFuncionalidade("following", atributo.FollowingTag, funcionalidade)}";
            }

            return XPath;
        }
        private static string FormataXPathPreceding(this ExtracaoAttribute atributo)
        {
            string XPath = string.Empty;
            if (!string.IsNullOrEmpty(atributo.PrecedingTag))
            {
                string funcionalidade = RetornaFuncionalidade(atributoClasse, atributo.PrecedingClass, condicao: true);
                funcionalidade += RetornaFuncionalidade(atributoId, atributo.PrecedingId, condicao: true);
                funcionalidade += RetornaFuncionalidade(atributoTexto, atributo.PrecedingText, condicao: true);
                //  Tag Preceding
                XPath = $"{RetornaCondicao(true)}{RetornaFuncionalidade("preceding", atributo.PrecedingTag, funcionalidade)}";
            }

            return XPath;
        }
    }
}