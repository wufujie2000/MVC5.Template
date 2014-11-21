using Datalist;
using MvcTemplate.Components.Datalists;
using MvcTemplate.Components.Mvc;
using MvcTemplate.Components.Security;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MvcTemplate.Controllers
{
    [AllowUnauthorized]
    public class DatalistController : BaseController
    {
        private IUnitOfWork unitOfWork;
        private Boolean disposed;

        public DatalistController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public virtual JsonResult GetData(AbstractDatalist datalist, DatalistFilter filter, Dictionary<String, Object> additionalFilters = null)
        {
            filter.AdditionalFilters = additionalFilters ?? filter.AdditionalFilters;
            datalist.CurrentFilter = filter;

            return Json(datalist.GetData(), JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        public JsonResult Role(DatalistFilter filter)
        {
            return GetData(new BaseDatalist<Role, RoleView>(unitOfWork), filter);
        }

        protected override void Dispose(Boolean disposing)
        {
            if (disposed) return;

            unitOfWork.Dispose();
            disposed = true;

            base.Dispose(disposing);
        }
    }
}
