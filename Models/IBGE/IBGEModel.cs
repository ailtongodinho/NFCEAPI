using NFCE.API.Helpers;

namespace NFCE.API.Models.IBGE
{
    public class IbgeModel
    {
        public long IdLocalidade { get; set; }
        public long Id { get; set; }
        public string Nome { get; set; }
        public string NomeSemAcento { get { return Nome.RemoveAccents(); } }
        public string Sigla { get; set; }
    }
    public class IbgeEstado : IbgeModel
    {
        public IbgeModel Regiao { get; set; }
    }
    public class IbgeMesorregiao : IbgeModel
    {
        public IbgeEstado UF { get; set; }
    }
    public class IbgeMicrorregiao : IbgeModel
    {
        public IbgeMesorregiao Mesorregiao { get; set; }
    }
    public class IbgeMunicipio : IbgeModel
    {
        public IbgeMicrorregiao Microrregiao { get; set; }
    }
    public class IbgeDistrito : IbgeModel
    {
        public IbgeMunicipio Municipio { get; set; }
    }
}