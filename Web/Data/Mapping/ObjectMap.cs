using AutoMapper;
using Template.Objects;

namespace Template.Data.Mapping
{
    public static class ObjectMapper
    {
        public static void MapObjects()
        {
            MapUsers();
            MapRoles();

            MapSystem();
        }

        #region Administration

        private static void MapUsers()
        {
            Mapper.CreateMap<Account, ProfileView>();

            Mapper.CreateMap<Account, AccountView>();
            Mapper.CreateMap<AccountView, Account>();

            Mapper.CreateMap<Account, UserView>();
            Mapper.CreateMap<UserView, Account>()
                .ForMember(account => account.UserId, property => property.MapFrom(view => view.Id));

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
        }

        private static void MapRoles()
        {
            Mapper.CreateMap<Role, RoleView>();
            Mapper.CreateMap<RoleView, Role>();

            Mapper.CreateMap<RolePrivilege, RolePrivilegeView>();
            Mapper.CreateMap<RolePrivilegeView, RolePrivilege>();

            Mapper.CreateMap<Privilege, PrivilegeView>();
            Mapper.CreateMap<PrivilegeView, Privilege>();

            Mapper.CreateMap<PrivilegeLanguage, PrivilegeLanguageView>();
            Mapper.CreateMap<PrivilegeLanguageView, PrivilegeLanguage>();
        }

        #endregion

        #region System

        private static void MapSystem()
        {
            Mapper.CreateMap<Language, LanguageView>();
            Mapper.CreateMap<LanguageView, Language>();
        }

        #endregion
    }
}
