using ApiApplication.Contracts;
using Contracts;
using Entities.ErrorModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace Server.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, 
            ILoggerManager logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>//выскочила ошибка,middleware начинает работу ,передается контекст запроса(где возникал ошибка)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;//записывается код ошибки
                    context.Response.ContentType = "application/json";//формат контента ощибки
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();//достается информация и путь ошибки
                    if (contextFeature != null)//если ошибка действительно есть
                    {
                        logger.LogError($"Something went wrong: {contextFeature.Error}");//передаем в логгер сообщение
                        await context.Response.WriteAsync(new ErrorDetails()//отправляем ответ(используя нашу модель ErrorDetails),перед этим сериализуем
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Internal Server Error."
                        }.ToString());
                    }
                });
            });
        }

    }
}
