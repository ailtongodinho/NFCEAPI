using System;
using System.Collections.Generic;
using System.Linq;
using NFCE.API.Attributes;

namespace NFCE.API.Models
{
    public class ExtracaoPatternsModel
    {
        public const string Delimiter = @"|&&|";
        public const string RemoveNovaLinha = @"\n";
        public const string RemoveTab = @"\t";
        public const string RemoveCarrier = @"\r";
        public const string Float = @"\d+?(\,|\.)\d+|\d+";
        public const string Int = @"\d+";
        public const string String = @"\w+";
        public const string AposDoisPontos = @"[^\:]*$";
        public const string AposDoisPontosInt = AposDoisPontos + Delimiter + Int;
        public const string AposDoisPontosString = AposDoisPontos + Delimiter + String;
        public const string AposDoisPontosFloat = AposDoisPontos + Delimiter + Float;
        public const string DataHora = @"(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}( \d{1,2}[:-]\d{2}([:-]\d{2,3})*)?";
    }
}