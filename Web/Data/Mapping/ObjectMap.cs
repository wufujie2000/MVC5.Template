using AutoMapper;
using Template.Objects;

namespace Template.Components.Services.AutoMapper
{
    public static class ModelMapping
    {
        public static void MapModels()
        {
            MapUsers();
            MapRoles();
        }

        #region Administration

        private static void MapUsers()
        {
            Mapper.CreateMap<Account, ProfileView>();

            Mapper.CreateMap<Account, AccountView>();
            Mapper.CreateMap<AccountView, Account>();

            Mapper.CreateMap<User, UserView>()
                .ForMember(view => view.UserFirstName, property => property.MapFrom(user => user.FirstName))
                .ForMember(view => view.UserLastName, property => property.MapFrom(user => user.LastName))
                .ForMember(view => view.UserDateOfBirth, property => property.MapFrom(user => user.DateOfBirth))
                .ForMember(view => view.UserRoleId, property => property.MapFrom(user => user.RoleId));
            Mapper.CreateMap<UserView, User>()
                .ForMember(user => user.FirstName, property => property.MapFrom(view => view.UserFirstName))
                .ForMember(user => user.LastName, property => property.MapFrom(view => view.UserLastName))
                .ForMember(user => user.DateOfBirth, property => property.MapFrom(view => view.UserDateOfBirth))
                .ForMember(user => user.RoleId, property => property.MapFrom(view => view.UserRoleId));

            Mapper.CreateMap<Account, UserView>();
            Mapper.CreateMap<UserView, Account>()
                .ForMember(account => account.UserId, property => property.MapFrom(view => view.Id));
        }

        private static void MapRoles()
        {
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
