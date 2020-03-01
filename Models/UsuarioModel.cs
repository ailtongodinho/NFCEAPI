using System;
using System.ComponentModel.DataAnnotations;

namespace NFCE.API.Models
{
    public class UsuarioModel
    {
        public int Id { get; internal set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo obrigatório")]
        public string Nome { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo obrigatório")]
        public string Sobrenome { get; set; }
        // [Required(AllowEmptyStrings = false, ErrorMessage = "Campo obrigatório")]
        public string RegistroNacional { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo obrigatório")]
        public string Sexo { get; set; }
        [EmailAddress]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo obrigatório")]
        public string Email { get; set; }
        public bool Ativo { get; internal set; }
        public DateTime DataInsercao { get; internal set; }
    }
}