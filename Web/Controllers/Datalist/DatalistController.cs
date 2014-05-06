using Datalist;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Template.Components.Datalists;
using Template.Components.Security;
using Template.Data.Core;
using Template.Objects;

namespace Template.Controllers.Datalist
{
    [AllowUnauthorized]
    public class DatalistController : BaseController
    {
        private IUnitOfWork unitOfWork;

        public DatalistController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public virtual JsonResult GetData(AbstractDatalist datalist, DatalistFilter filter, Dictionary<String, Object> additionalFilters = null)
        {
            datalist.CurrentFilter = filter;
            filter.AdditionalFilters = additionalFilters ?? filter.AdditionalFilters;

            return Json(datalist.GetData(), JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult Role(DatalistFilter filter)
        {
            return GetData(new BaseDatalist<Role, RoleView>(unitOfWork), filter);
        }
    }
}
