using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NFCE.API.Helpers;
using NFCE.API.Interfaces.Services;
using NFCE.API.Models;
using NFCE.API.Models.IBGE;

namespace NFCE.API.Services
{
    public class IBGEService
    {
        private HttpClient Client { get; set; }
        public List<IbgeEstado> Estados { get; set; }
        public IBGEService(IConfiguration config)
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri(config.GetValue<string>("IBGE:BaseAddress"));
            //  Pegando UF's
            GetUF();
        }
        ~IBGEService()
        {
            Client.Dispose();
        }
        private async Task<List<T>> Consultar<T>(string uri)
        {
            var response = await Client.GetAsync(uri);
            var content = await response.Content.ReadAsStringAsync();
            // var lista = JsonConvert.DeserializeObject(content);
            return JsonConvert.DeserializeObject<List<T>>(content);
        }
        public async void GetUF()
        {
            Estados = await Consultar<IbgeEstado>("estados");
        }
        public async Task<IbgeMunicipio> GetMunicipioPorUF(long ufId, string municipio)
        {
            var lista = await Consultar<IbgeMunicipio>($"estados/{ufId}/municipios");
            return lista.Where(x => x.NomeSemAcento.ToLower() == municipio.RemoveAccents().ToLower()).FirstOrDefault();
        }
        public async Task<IbgeMunicipio> GetMunicipioPorId(long ufId, string municipioId)
        {
            IbgeMunicipio retorno = null;
            if (long.TryParse(municipioId, out long id))
            {
                var lista = await Consultar<IbgeMunicipio>($"estados/{ufId}/municipios/{id}");
                retorno = lista.FirstOrDefault();
            }
            return retorno;
        }
        public async Task<IbgeDistrito> GetDistritoPorMunicipio(long municipioId, string distrito)
        {
            var lista = await Consultar<IbgeDistrito>($"municipios/{municipioId}/distritos");
            return lista.Where(x => x.NomeSemAcento.ToLower() == distrito.RemoveAccents().ToLower()).FirstOrDefault();
        }
        public async Task<IbgeDistrito> GetDistrito(string distritoId)
        {
            IbgeDistrito retorno = null;
            if (long.TryParse(distritoId, out long id))
            {
                var lista = await Consultar<IbgeDistrito>($"distritos/{id}");
                retorno = lista.FirstOrDefault();
            }
            return retorno;
        }
        public LocalidadeModel GetLocalidade(EmissorModel model)
        {
            LocalidadeModel localidade = new LocalidadeModel();
            var estado = Estados.Where(x => x.Sigla.ToLower() == model.UF.ToLower()).FirstOrDefault();
            //  Tenta pegar pelo Id
            if (estado == null) Estados.Where(x => x.Id.ToString().ToLower() == model.UF.Trim().ToLower()).FirstOrDefault();
            if (estado != null)
            {
                //  UF
                localidade.UFId = int.Parse(estado.Id.ToString());
                localidade.UFNome = estado.Nome;
                localidade.UFSigla = estado.Sigla;
                //  Região
                localidade.RegiaoId = int.Parse(estado.Regiao.Id.ToString());
                localidade.RegiaoNome = estado.Regiao.Nome;
                localidade.RegiaoSigla = estado.Regiao.Sigla;
                //  Pesquisa Municipio
                var municipio = GetMunicipioPorUF(estado.Id, model.Municipio).Result;
                //  Tenta pegar pelo Id
                if (municipio == null) municipio = GetMunicipioPorId(estado.Id, model.Municipio).Result;
                if (municipio != null)
                {
                    localidade.MicrorregiaoId = int.Parse(municipio.Microrregiao.Id.ToString());
                    localidade.MicrorregiaoNome = municipio.Microrregiao.Nome;
                    localidade.MesorregiaoId = int.Parse(municipio.Microrregiao.Mesorregiao.Id.ToString());
                    localidade.MesorregiaoNome = municipio.Microrregiao.Mesorregiao.Nome;
                    localidade.MunicipioId = municipio.Id;
                    localidade.MunicipioNome = municipio.Nome;
                    var distrito = GetDistritoPorMunicipio(municipio.Id, model.Distrito).Result;
                    //  Tenta pegar pelo Id
                    if (distrito == null) distrito = GetDistrito(model.Distrito).Result;
                    if (distrito != null)
                    {
                        localidade.DistritoId = distrito.Id;
                        localidade.DistritoNome = distrito.Nome;
                    }
                }
            }
            return localidade;
        }
    }
}