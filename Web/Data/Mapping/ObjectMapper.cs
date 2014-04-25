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
            Mapper.CreateMap<Person, PersonView>();
            Mapper.CreateMap<PersonView, Person>();

            Mapper.CreateMap<Account, AccountView>();
            Mapper.CreateMap<AccountView, Account>();

            Mapper.CreateMap<Account, ProfileView>();
            Mapper.CreateMap<ProfileView, Account>();

            Mapper.CreateMap<Account, UserView>();
            Mapper.CreateMap<UserView, Account>()
                .AfterMap((user, account) => {
                    account.Person.Id = account.Id;
                    account.PersonId = account.Id;
                });
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

        #region System

        private static void MapSystem()
        {
            Mapper.CreateMap<Language, LanguageView>();
            Mapper.CreateMap<LanguageView, Language>();
        }

        #endregion
    }
}
