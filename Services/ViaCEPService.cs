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
using NFCE.API.Models.ViaCEP;

namespace NFCE.API.Services
{
    public class ViaCEPService
    {
        private HttpClient Client { get; set; }
        public ViaCEPService(IConfiguration config)
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri(config.GetValue<string>("ViaCEP:BaseAddress"));
        }
        ~ViaCEPService()
        {
            Client.Dispose();
        }
        private async Task<T> ConsultarJson<T>(string uri)
        {
            var response = await Client.GetAsync($"{uri}/json");
            var content = await response.Content.ReadAsStringAsync();
            // var lista = JsonConvert.DeserializeObject(content);
            return JsonConvert.DeserializeObject<T>(content);
        }
        public Task<ViaCEPModel> ConsultaCEP(string CEP)
        {
            return ConsultarJson<ViaCEPModel>(CEP);
        }
    }
}