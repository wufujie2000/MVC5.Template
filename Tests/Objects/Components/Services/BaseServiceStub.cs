using Template.Components.Services;
using Template.Data.Core;

namespace Template.Tests.Objects.Components.Services
{
    public class BaseServiceStub : BaseService
    {
        public IUnitOfWork BaseUnitOfWork
        {
            get
            {
                return UnitOfWork;
            }
        }

        public BaseServiceStub(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
