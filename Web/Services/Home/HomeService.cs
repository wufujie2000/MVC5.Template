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
            AlertMessages.AddError(Messages.Unauthorized);
        }

        public void AddSystemErrorMessage()
        {
            AlertMessages.AddError(Messages.SystemError);
        }

        public void AddPageNotFoundMessage()
        {
            AlertMessages.AddError(Messages.PageNotFound);
        }
    }
}
