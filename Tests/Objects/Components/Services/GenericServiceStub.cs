using System.Web.Mvc;
using Template.Components.Services;
using Template.Data.Core;
using Template.Objects;

namespace Template.Tests.Objects.Components.Services
{
    public class GenericServiceStub : GenericService<User, UserView>
    {
        public IUnitOfWork BaseUnitOfWork
        {
            get
            {
                return UnitOfWork;
            }
        }
        public ModelStateDictionary BaseModelState
        {
            get
            {
                return ModelState;
            }
        }

        public GenericServiceStub(ModelStateDictionary modelState)
            : base(modelState)
        {
        }
    }
}
