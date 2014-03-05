using System;
using System.Web;
using System.Web.Mvc;
using Template.Components.Alerts;
using Template.Data.Core;

namespace Template.Components.Services
{
    public abstract class BaseService : IService, IDisposable
    {
        private Boolean disposed;
        private HttpContextBase httpContext;

        public ModelStateDictionary ModelState
        {
            get;
            set;
        }
        public HttpContextBase HttpContext
        {
            get
            {
                return httpContext ?? (httpContext = new HttpContextWrapper(System.Web.HttpContext.Current));
            }
            set
            {
                httpContext = value;
            }
        }
        
        protected IUnitOfWork UnitOfWork
        {
            get;
            set;
        }

        public String CurrentAccountId
        {
            get
            {
                return HttpContext.User.Identity.Name;
            }
        }
        public String CurrentLanguage
        {
            get
            {
                return HttpContext.Request.RequestContext.RouteData.Values["language"] as String;
            }
        }
        public String CurrentArea
        {
            get
            {
                return HttpContext.Request.RequestContext.RouteData.Values["area"] as String;
            }
        }
        public String CurrentController
        {
            get
            {
                return HttpContext.Request.RequestContext.RouteData.Values["controller"] as String;
            }
        }
        public String CurrentAction
        {
            get
            {
                return HttpContext.Request.RequestContext.RouteData.Values["action"] as String;
            }
        }

        public MessagesContainer AlertMessages
        {
            get;
            set;
        }

        public BaseService(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }
        protected virtual void Dispose(Boolean disposing)
        {
            if (disposed) return;
            UnitOfWork.Dispose();
            UnitOfWork = null;
            disposed = true;
        }
    }
}
