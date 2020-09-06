using System;
using System.IO;
using System.Net;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NFCE.API.Interfaces.Services;
using NFCE.API.Models;
using NFCE.API.Models.Handler;

namespace NFCE.API.Helpers
{
    public class ControllerActionFilter : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {

        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            //  Tratando a excessão
            context.ExceptionHandled = true;
            //  Sempre IActionResult
            var propriedade = context.Result?.GetType().GetProperty("Value");
            //  StatusCode
            HttpStatusCode statusCode = context.Exception == null ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
            //  Diferenciando Excessões de usuário
            string mensagem = context.Exception == null ? null : "Ops! Aconteceu algum erro com o servidor!\nPor favor, tente novamente mais tarde.";
            if (context.Exception is HttpExceptionHandler)
            {
                HttpExceptionHandler excessaoUsuario = (HttpExceptionHandler)context.Exception;
                mensagem = excessaoUsuario.Message;
                statusCode = excessaoUsuario.StatusCode;
            }
            //  Criando resultado
            var resultado = new ResponseModel
            {
                Sucesso = context.Exception == null,
                HttpStatus = statusCode,
                Mensagem = mensagem,
                Error = (context.Exception?.InnerException ?? context.Exception)?.Message,
                Objeto = context.Result != null ? propriedade.GetValue(context.Result) : null
            };
            //  Criando objeto de resposta
            var httpResult = new ObjectResult(resultado);
            httpResult.StatusCode = (int)statusCode;
            context.Result = httpResult;
        }
    }
}