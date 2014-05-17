using AutoMapper;
using Template.Objects;

namespace Template.Data.Mapping
{
    public static class ObjectMapper
    {
        public static void MapObjects()
        {
            MapAccounts();
            MapRoles();

            MapSystem(); // TOOD: Mapping should be done in objects assembly, by using profiles
        }

        #region Administration

        private static void MapAccounts()
        {
            Mapper.CreateMap<Person, PersonView>();
            Mapper.CreateMap<PersonView, Person>();

            Mapper.CreateMap<Account, AccountView>();
            Mapper.CreateMap<AccountView, Account>();

            Mapper.CreateMap<Akkount, AkkountView>();
            Mapper.CreateMap<AkkountView, Akkount>();

            Mapper.CreateMap<Account, ProfileView>();
            Mapper.CreateMap<ProfileView, Account>();
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
