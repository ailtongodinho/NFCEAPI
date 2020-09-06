using System;
using System.ComponentModel.DataAnnotations;

namespace NFCE.API.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo Nome obrigatório")]
        public string Nome { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo Sobrenome obrigatório")]
        public string Sobrenome { get; set; }
        // [Required(AllowEmptyStrings = false, ErrorMessage = "Campo Registro Nacional obrigatório")]
        public string RegistroNacional { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo Sexo é obrigatório")]
        public string Sexo { get; set; }
        [EmailAddress]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo Email é obrigatório")]
        public string Email { get; set; }
        public string UF { get; set; }
        public string Municipio { get; set; }
        public string CEP { get; set; }
        public bool Ativo { get; internal set; }
        public DateTime DataInsercao { get; internal set; }
    }
}