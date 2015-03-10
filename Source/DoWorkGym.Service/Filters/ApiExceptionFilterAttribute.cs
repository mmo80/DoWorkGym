using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;
using DoWorkGym.Util;

namespace DoWorkGym.Service.Filters
{
    public class ApiExceptionFilterAttribute : System.Web.Http.Filters.ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            // Base error filter
            var httpResponseException = new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("An error occurred, please try again or contact the administrator."),
                ReasonPhrase = "Critical Exception"
            });

            if (actionExecutedContext == null)
            {
                Logging.Error("actionExecutedContext is null.");
                throw httpResponseException;
            }

            if (actionExecutedContext.Exception == null)
            {
                Logging.Error("actionExecutedContext.Exception is null.");
                throw httpResponseException;
            }

            Exception exception = actionExecutedContext.Exception;
            string requestedUri = null;

            if (actionExecutedContext.Request != null && actionExecutedContext.Request.RequestUri != null)
            {
                requestedUri = actionExecutedContext.Request.RequestUri.ToString();
            }

            var errorMessage = string.Format("RequestUri:{0}, Error:{1}", requestedUri, exception.Message);

            // Log error!
            Logging.Error(errorMessage, actionExecutedContext.Exception);

            // ********** Example for custom Exception errors!!!
            //if (exception is ValueValidationException)
            //{
            //    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NoContent)
            //    {
            //        //Content = new StringContent(exception.Message),
            //        ReasonPhrase = exception.Message
            //    });
            //}

            //if (exception is FormatException)
            //{
            //    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NoContent)
            //    {
            //        //Content = new StringContent(exception.Message),
            //        ReasonPhrase = exception.Message
            //    });
            //}

            throw httpResponseException;
        }
    }
}
