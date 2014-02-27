using System;
using System.IO;
using System.Web;

namespace Tests.Helpers
{
    public class HttpContextStub
    {
        public HttpResponse Response
        {
            get;
            set;
        }
        public HttpRequest Request
        {
            get;
            set;
        }
        public HttpContext Context
        {
            get;
            set;
        }

        public HttpContextStub()
        {
            Request = new HttpRequest(String.Empty, "http://localhost:19174/", String.Empty);
            Response = new HttpResponse(new StringWriter());
            Context = new HttpContext(Request, Response);
        }
    }
}