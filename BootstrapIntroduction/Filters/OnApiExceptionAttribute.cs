using BootstrapIntroduction.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace BootstrapIntroduction.Filters
{
    public class OnApiExceptionAttribute : ExceptionFilterAttribute
    {

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {

            var exceptionType = actionExecutedContext.Exception.GetType().Name;

            ReturnData returnData;

            switch (exceptionType)
            {

                case "ObjectNotFoundException":
                    returnData = new ReturnData(HttpStatusCode.NotFound, actionExecutedContext.Exception.Message, "Error");
                    break;
                default:
                    returnData = new ReturnData(HttpStatusCode.InternalServerError, "An error occurred, please try again or contact the administrator.", "Error");
                    break;
            }

            actionExecutedContext.Response = new HttpResponseMessage(returnData.HttpStatusCode)
            {
                Content = new StringContent(returnData.Content),
                ReasonPhrase = returnData.ReasonPhrase
            };
        }
    }
}