using Template.Data.Core;
namespace Template.Services
{
    public class HomeService : BaseService, IHomeService
    {
        public HomeService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
