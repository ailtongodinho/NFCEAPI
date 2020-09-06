namespace NFCE.API.Models
{
    public class LocalidadeModel
    { 
        public long Id { get; set; }
        public int RegiaoId { get; set; }
        public string RegiaoSigla { get; set; }
        public string RegiaoNome { get; set; }
        public int UFId { get; set; }
        public string UFSigla { get; set; }
        public string UFNome { get; set; }
        public int MesorregiaoId { get; set; }
        public string MesorregiaoNome { get; set; }
        public int MicrorregiaoId { get; set; }
        public string MicrorregiaoNome { get; set; }
        public long MunicipioId { get; set; }
        public string MunicipioNome { get; set; }
        public long DistritoId { get; set; }
        public string DistritoNome { get; set; }
    }
}