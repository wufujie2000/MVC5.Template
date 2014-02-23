using AutoMapper;
using Template.Objects;

namespace Template.Components.Services.AutoMapper
{
    public static class ModelMapping
    {
        public static void MapModels()
        {
            MapAdministration();
        }

        #region Administration

        private static void MapAdministration()
        {
            Mapper.CreateMap<Account, ProfileView>();

            Mapper.CreateMap<Account, AccountView>();
            Mapper.CreateMap<AccountView, Account>();

            Mapper.CreateMap<User, UserView>();
            Mapper.CreateMap<UserView, User>()
                .ForMember(user => user.FirstName, property => property.MapFrom(view => view.UserFirstName))
                .ForMember(user => user.LastName, property => property.MapFrom(view => view.UserLastName))
                .ForMember(user => user.DateOfBirth, property => property.MapFrom(view => view.UserDateOfBirth))
                .ForMember(user => user.RoleId, property => property.MapFrom(view => view.UserRoleId));

            Mapper.CreateMap<Account, UserView>();
            Mapper.CreateMap<UserView, Account>()
                .ForMember(account => account.UserId, property => property.MapFrom(view => view.Id));

            Mapper.CreateMap<Role, RoleView>();
            Mapper.CreateMap<RoleView, Role>();

            Mapper.CreateMap<RolePrivilege, RolePrivilegeView>();
            Mapper.CreateMap<RolePrivilegeView, RolePrivilege>();

            Mapper.CreateMap<Privilege, PrivilegeView>();
            Mapper.CreateMap<PrivilegeView, Privilege>();
        }

        #endregion
    }
}
