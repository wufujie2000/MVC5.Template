using Datalist;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Template.Components.Datalists;
using Template.Components.Security;

namespace Template.Controllers.Datalist
{
    [AllowUnauthorized]
    public class DatalistController : BaseController
    {
        public virtual JsonResult GetData(AbstractDatalist datalist, DatalistFilter filter, Dictionary<String, Object> additionalFilters = null)
        {
            datalist.CurrentFilter = filter;
            filter.AdditionalFilters = additionalFilters ?? filter.AdditionalFilters;
            return Json(datalist.GetData(), JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult Roles(DatalistFilter filter)
        {
            return GetData(new RolesDatalist(), filter);
        }
    }
}
