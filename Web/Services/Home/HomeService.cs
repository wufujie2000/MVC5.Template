using System;
using Template.Components.Alerts;
using Template.Data.Core;
using Template.Resources.Shared;

namespace Template.Services
{
    public class HomeService : BaseService, IHomeService
    {
        public HomeService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
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
