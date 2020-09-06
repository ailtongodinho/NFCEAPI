using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using NFCE.API.Interfaces.Repositories;
using NFCE.API.Interfaces.Services;
using NFCE.API.Models;
using NFCE.API.Models.Request;
using NFCE.API.Models.Response.Emissor;

namespace NFCE.API.Services
{
    public class EmissorService : IEmissorService
    {
        private readonly IEmissorRepository _EmissorRepository;
        private readonly ILocalidadeService _LocalidadeService;
        private readonly ViaCEPService _ViaCEPService;
        public EmissorService(
            IEmissorRepository EmissorRepository,
            ILocalidadeService localidadeService,
            ViaCEPService viaCEPService
        )
        {
            _EmissorRepository = EmissorRepository;
            _LocalidadeService = localidadeService;
            _ViaCEPService = viaCEPService;
        }
        #region CRUD
        /// <summary>
        /// Atualizar
        /// </summary>
        /// <returns>Sucesso da Atualizar</returns>
        public bool Atualizar(EmissorModel model)
        {
            return _EmissorRepository.Update(model);
        }
        /// <summary>
        /// Consultar
        /// </summary>
        /// <returns>Modelo de Consulta</returns>
        public EmissorModel Consultar(int Id)
        {
            return _EmissorRepository.Consultar(Id);
        }
        /// <summary>
        /// Consultar por Modelo
        /// </summary>
        /// <returns>Modelo de Consulta</returns>
        public EmissorModel Consultar(EmissorModel model)
        {
            return _EmissorRepository.Consultar(model);
        }
        /// <summary>
        /// Deletar
        /// </summary>
        /// <returns>Verdadeiro ou Falso</returns>
        public bool Deletar(int Id)
        {
            return _EmissorRepository.Delete(Id);
        }
        /// <summary>
        /// Novo Registro
        /// </summary>
        /// <param name="modelo">Modelo de Emissor</param>
        /// <returns>Identificação do Registro adicionado</returns>
        public int Novo(EmissorModel modelo)
        {
            EmissorModel modeloViaCEP = null;
            //  Completa informações
            if (!string.IsNullOrEmpty(modelo.CEP))
            {
                modeloViaCEP = modelo;
                //  Consulta o CEP
                var cep = _ViaCEPService.ConsultaCEP(modeloViaCEP.CEP).Result;
                //  Atribui o Municipio
                modeloViaCEP.Municipio = cep.Localidade;
                modeloViaCEP.Distrito = cep.Bairro;
                modeloViaCEP.Logradouro = cep.Logradouro;
                modeloViaCEP.UF = cep.Uf;
            }
            //  Procura pela Localidade
            try
            {
                List<EmissorModel> listaEmissor = new List<EmissorModel>();
                listaEmissor.Add(modelo);
                if (modeloViaCEP != null) listaEmissor.Add(modeloViaCEP);
                LocalidadeModel localidade = _LocalidadeService.Consulta(listaEmissor);
                if (localidade != null)
                {
                    if (string.IsNullOrEmpty(localidade.DistritoNome))
                    {
                        localidade.DistritoNome = modeloViaCEP.Distrito;
                        localidade.DistritoId = -1;
                    }
                    modelo.IdLocalidade = _LocalidadeService.Novo(localidade);
                }
            }
            catch (Exception ex)
            {
                //  LOG
            }
            //  Aplica a data de Inserção
            modelo.DataInsercao = DateTime.Now;

            //  Adiciona Emissor
            return _EmissorRepository.Adicionar(modelo);
        }
        #endregion
        #region Metodos
        /// <summary>
        /// Retorna agregado
        /// </summary>
        /// <param name="IdUsuario">Identificação do Usuário</param>
        /// <param name="emissorAgregadoRequest">Parâmetros para listar Emissor Agregado</param>
        /// <returns>Agregado</returns>
        public IEnumerable<EmissorAgregadoResponse> Agregado(int IdUsuario, EmissorAgregadoRequest emissorAgregadoRequest)
        {
            emissorAgregadoRequest.DataInicial = emissorAgregadoRequest.DataInicial ?? DateTime.Now.AddDays(-60);
            emissorAgregadoRequest.DataFinal = emissorAgregadoRequest.DataFinal ?? DateTime.Now;
            return _EmissorRepository.Agregado(IdUsuario, emissorAgregadoRequest);
        }
        #endregion
    }
}