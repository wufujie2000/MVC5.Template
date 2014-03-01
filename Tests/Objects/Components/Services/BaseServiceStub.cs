using System.Web.Mvc;
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

        public BaseServiceStub()
            : base()
        {
        }
        public BaseServiceStub(ModelStateDictionary modelState)
            : base(modelState)
        {
        }
    }
}
