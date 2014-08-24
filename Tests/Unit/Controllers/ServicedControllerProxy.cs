using MvcTemplate.Controllers;
using MvcTemplate.Services;

namespace MvcTemplate.Tests.Unit.Controllers
{
    public class ServicedControllerProxy : ServicedController<IService>
    {
        public IService BaseService
        {
            get
            {
                return Service;
            }
        }

        public ServicedControllerProxy(IService service)
            : base(service)
        {
        }
    }
}
