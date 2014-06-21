using AutoMapper;
using Template.Objects;

namespace Template.Data.Mapping
{
    public static class ObjectMapper
    {
        public static void MapObjects()
        {
            MapAuth();

            MapAccounts();
            MapRoles();
        }

        #region Auth

        private static void MapAuth()
        {
            Mapper.CreateMap<LoginView, Account>();
        }

        #endregion

        #region Administration

        private static void MapAccounts()
        {
            Mapper.CreateMap<Account, AccountView>();
            Mapper.CreateMap<AccountView, Account>();

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
    }
}
