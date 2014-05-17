using System;
using Template.Objects;
using Template.Data.Core;

namespace Template.Services
{
    public class AkkountsService : GenericService<Akkount, AkkountView>, IAkkountsService
    {
        public AkkountsService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        // TODO: Add service implementation
    }
}
