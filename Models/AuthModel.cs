using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace NFCE.API.Models
{
    public class AuthModel
    {
        [JsonIgnore]
        public int Id { get; set; }
        [JsonIgnore]
        public int IdUsuario { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo obrigatório")]
        public string Login { get; set; }
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$", ErrorMessage = "Senha deve conter 8 caracteres com 1 maiusculo, 1 minusculo e 1 número")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo obrigatório")]
        public string Senha { get; set; }
        [JsonIgnore]
        public bool Valido { get; set; }
        [JsonIgnore]
        public DateTime DataUltimo { get; set; }
    }
}