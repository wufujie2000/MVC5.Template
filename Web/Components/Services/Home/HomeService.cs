using Template.Resources.Shared;
using System;
using System.Web.Mvc;

namespace Template.Components.Services
{
    public class HomeService : BaseService
    {
        public HomeService(ModelStateDictionary modelState)
            : base(modelState)
        {
        }

        public void AddUnauthorizedMessage()
        {
            AlertMessages.AddError(String.Empty, Messages.Unauthorized, 0);
        }

        public void AddSystemErrorMessage()
        {
            AlertMessages.AddError(String.Empty, Messages.SystemError, 0);
        }

        public void AddPageNotFoundMessage()
        {
            AlertMessages.AddError(String.Empty, Messages.PageNotFound, 0);
        }
    }
}
