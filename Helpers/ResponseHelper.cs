using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NFCE.API.Models;
using NFCE.API.Models.Handler;

namespace NFCE.API.Helpers
{
    public static class ResponseHelper
    {
        public static IActionResult TratamentoModelState(ActionContext context)
        {
            return new BadRequestObjectResult(new ResponseModel
            {
                Sucesso = false,
                Mensagem = string.Join("\n", context.ModelState.Values.Select(x => string.Join("\n", x.Errors.Select(y => y.ErrorMessage))))
            });
        }
        public static void TratamentoRespostaPadrao(ref ActionExecutedContext context)
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
                Error = context.Exception,
                Objeto = context.Result != null ? propriedade.GetValue(context.Result) : null
            };
            //  Criando objeto de resposta
            var httpResult = new ObjectResult(resultado);
            httpResult.StatusCode = (int)statusCode;
            context.Result = httpResult;
        }
    }
}