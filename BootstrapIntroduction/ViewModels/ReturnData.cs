using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace BootstrapIntroduction.ViewModels
{
    public class ReturnData
    {

        public ReturnData(HttpStatusCode httpStatusCode, string content, string reasonPhrase)
        {

            HttpStatusCode = httpStatusCode;
            Content = content;
            ReasonPhrase = reasonPhrase;

        }


        public HttpStatusCode HttpStatusCode { get; private set; }
        public string Content { get; private set; }
        public string ReasonPhrase { get; private set; }
    }
}