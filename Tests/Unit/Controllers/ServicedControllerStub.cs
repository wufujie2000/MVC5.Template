using MvcTemplate.Controllers;
using MvcTemplate.Services;

namespace MvcTemplate.Tests.Unit.Controllers
{
    public class ServicedControllerStub : ServicedController<IService>
    {
        public IService BaseService
        {
            get
            {
                return Service;
            }
        }

        public ServicedControllerStub(IService service)
            : base(service)
        {
        }
    }
}
