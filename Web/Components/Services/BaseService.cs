using System;
using System.Web;
using System.Web.Mvc;
using Template.Components.Alerts;
using Template.Data.Core;

namespace Template.Components.Services
{
    public abstract class BaseService : IDisposable
    {
        private Boolean disposed;
        protected IUnitOfWork UnitOfWork
        {
            get;
            set;
        }

        public String CurrentAccountId
        {
            get
            {
                return HttpContext.Current.User.Identity.Name;
            }
        }
        public String CurrentLanguage
        {
            get
            {
                return HttpContext.Current.Request.RequestContext.RouteData.Values["language"] as String;
            }
        }
        public String CurrentArea
        {
            get
            {
                return HttpContext.Current.Request.RequestContext.RouteData.Values["area"] as String;
            }
        }
        public String CurrentController
        {
            get
            {
                return HttpContext.Current.Request.RequestContext.RouteData.Values["controller"] as String;
            }
        }
        public String CurrentAction
        {
            get
            {
                return HttpContext.Current.Request.RequestContext.RouteData.Values["action"] as String;
            }
        }

        public IMessagesContainer AlertMessages
        {
            get;
            protected set;
        }

        public BaseService() : this(null)
        {
        }
        public BaseService(ModelStateDictionary modelState)
        {
            UnitOfWork = new UnitOfWork();
            AlertMessages = new MessagesContainer(modelState);
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
