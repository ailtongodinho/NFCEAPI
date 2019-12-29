using System.ComponentModel.DataAnnotations;

namespace NFCE.API.Models
{
    public class ExtracaoRequestModel
    {
        [Required]
        public string Url { get; set; }
    }
}