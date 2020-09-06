using System;
using System.Collections.Generic;

namespace NFCE.API.Extensions
{
    public static class PropertyExtension
    {
        public static List<string> RetornaNomePropriedade(object prop)
        {
            List<string> retorno = new List<string>();
            if(prop != null)
            {
                Type tipo = prop.GetType();
                foreach(var x in tipo.GetProperties())
                {
                    retorno.Add(x.Name);
                }
            }    
            return retorno;
        }
    }
}