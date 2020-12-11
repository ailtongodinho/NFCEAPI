using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using NFCE.API.Interfaces.Repositories;
using NFCE.API.Interfaces.Services;
using NFCE.API.Models;
using NFCE.API.Models.IBGE;

namespace NFCE.API.Services
{
    public class LocalidadeService : ILocalidadeService
    {
        private readonly IConfiguration _config;
        private static ILocalidadeRepository _localidadeRepository;
        private readonly IBGEService _IBGEService;
        public LocalidadeService(ILocalidadeRepository localidadeRepository, IBGEService iBGEService)
        {
            _config = localidadeRepository.GetConfiguration;
            _localidadeRepository = localidadeRepository;
            _IBGEService = iBGEService;
        }
        public LocalidadeModel Consulta(int Id)
        {
            return _localidadeRepository.GetById(Id);
        }
        public LocalidadeModel Consulta(List<EmissorModel> models)
        {
            // return _IBGEService.GetLocalidade(emissorModel);
            LocalidadeModel localidade = new LocalidadeModel();
            IbgeEstado estado = null;
            //  Tenta encontrar o estado
            foreach (var item in models)
            {
                estado = _IBGEService.Estados.Where(x => x.Sigla.ToLower() == item.UF.ToLower()).FirstOrDefault();
                //  Tenta pegar pelo Id
                if (estado == null) _IBGEService.Estados.Where(x => x.Id.ToString().ToLower() == item.UF.Trim().ToLower()).FirstOrDefault();
                //  Quebra
                if (estado != null) break;
            }
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
                IbgeMunicipio municipio = null;
                //  Pesquisa Municipio
                foreach (var item in models)
                {
                    municipio = _IBGEService.GetMunicipioPorUF(estado.Id, item.Municipio).Result;
                    //  Tenta pegar pelo Id
                    if (municipio == null) municipio = _IBGEService.GetMunicipioPorId(estado.Id, item.Municipio).Result;
                    //  Quebra
                    if (municipio != null) break;
                }

                if (municipio != null)
                {
                    localidade.MicrorregiaoId = int.Parse(municipio.Microrregiao.Id.ToString());
                    localidade.MicrorregiaoNome = municipio.Microrregiao.Nome;
                    localidade.MesorregiaoId = int.Parse(municipio.Microrregiao.Mesorregiao.Id.ToString());
                    localidade.MesorregiaoNome = municipio.Microrregiao.Mesorregiao.Nome;
                    localidade.MunicipioId = municipio.Id;
                    localidade.MunicipioNome = municipio.Nome;
                    IbgeDistrito distrito = null;
                    foreach (var item in models)
                    {
                        distrito = _IBGEService.GetDistritoPorMunicipio(municipio.Id, item.Distrito).Result;
                        //  Tenta pegar pelo Id
                        if (distrito == null) distrito = _IBGEService.GetDistrito(item.Distrito).Result;
                        //  Quebra
                        if (distrito != null) break;
                    }
                    if (distrito != null)
                    {
                        localidade.DistritoId = distrito.Id;
                        localidade.DistritoNome = distrito.Nome;
                    }
                }
            }
            return localidade;
        }
        public LocalidadeModel Consulta(LocalidadeModel model)
        {
            return _localidadeRepository.GetList(x =>
                x.RegiaoId == model.RegiaoId &&
                x.UFId == model.UFId &&
                x.MesorregiaoId == model.MesorregiaoId &&
                x.MicrorregiaoId == model.MicrorregiaoId &&
                x.MunicipioId == model.MunicipioId
            ).Where(x =>
                (x.DistritoId == model.DistritoId || x.DistritoNome?.ToUpper() == model.DistritoNome?.ToUpper())
            ).FirstOrDefault();
        }
        public List<LocalidadeModel> Listar()
        {
            return _localidadeRepository.GetAll().ToList();
        }
        public List<IbgeEstado> ListarEstados()
        {
            return _localidadeRepository.ListarEstados();
        }
        public List<IbgeDistrito> ListarDistritos(int municipioId)
        {
            return _localidadeRepository.ListarDistritos(municipioId);
        }
        public List<IbgeMunicipio> ListarMunicipios(int uFId)
        {
            return _localidadeRepository.ListarMunicipios(uFId);
        }
        public long Novo(LocalidadeModel model)
        {
            var db_model = Consulta(model);
            if (db_model == null)
            {
                db_model = model;
                db_model.Id = _localidadeRepository.Insert(db_model);
            }
            return db_model.Id;
        }
        public bool Atualizar(LocalidadeModel model)
        {
            return _localidadeRepository.Update(model);
        }
    }
}