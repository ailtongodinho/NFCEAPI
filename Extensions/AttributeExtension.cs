using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace NFCE.API.Extensions
{
    public static class AttributeExtension
    {
        /// <summary>
        /// Retonar o atributo
        /// </summary>
        /// <param name="tipoClasse">Referencia a tipo(Type) da classe</param>
        /// <param name="propriedade">Nome da propriedade. Caso "null", retorna a propriedade da Classe</param>
        /// <typeparam name="T">Atributo a retornar</typeparam>
        /// <returns>Atributo da propriedade da Classe ou próprio da classe</returns>
        public static T RetornaAtributo<T>(this Type tipoClasse, string propriedade = null) where T : Attribute
        {
            T retorno;
            if (string.IsNullOrEmpty(propriedade))
            {
                retorno = tipoClasse.GetCustomAttribute<T>();
            }
            else
            {
                if (tipoClasse.IsEnum)
                    retorno = tipoClasse.GetMember(propriedade).First().GetCustomAttribute<T>();
                else
                    retorno = tipoClasse.GetProperty(propriedade).GetCustomAttribute<T>();
            }
            return retorno;
        }
        public static List<T> RetornaAtributos<T>(this Type tipoClasse, string propriedade = null) where T : Attribute
        {
            List<T> retorno = new List<T>();
            if (string.IsNullOrEmpty(propriedade))
            {
                retorno.AddRange(tipoClasse.GetCustomAttributes<T>());
            }
            else
            {
                if (tipoClasse.IsEnum)
                    retorno.AddRange(tipoClasse.GetMember(propriedade).Select(x => x.GetCustomAttribute<T>()));
                else
                    retorno.AddRange(tipoClasse.GetProperty(propriedade).GetCustomAttributes<T>());

            }
            return retorno;
        }
    }
}