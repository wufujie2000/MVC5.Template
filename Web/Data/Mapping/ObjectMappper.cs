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
                .ForMember(account => account.UserId, property => property.MapFrom(user => user.Id))
                .AfterMap((user, account) => {
                    account.User = new User();
                    account.User.Id = user.Id;
                    account.UserId = account.User.Id;
                    account.User.LastName = user.UserLastName;
                    account.User.FirstName = user.UserFirstName;
                    account.User.DateOfBirth = user.UserDateOfBirth;
                    if (user.UserRoleId != null)
                    {
                        account.User.Role = new Role();
                        account.User.Role.Id = user.UserRoleId;
                        account.User.RoleId = account.User.Role.Id;
                        account.User.Role.Name = user.UserRoleName;
                    }
                });

            Mapper.CreateMap<User, UserView>()
                .ForMember(view => view.UserDateOfBirth, property => property.MapFrom(user => user.DateOfBirth))
                .ForMember(view => view.UserFirstName, property => property.MapFrom(user => user.FirstName))
                .ForMember(view => view.UserLastName, property => property.MapFrom(user => user.LastName))
                .ForMember(view => view.UserRoleId, property => property.MapFrom(user => user.RoleId))
                .ForMember(view => view.UserRoleName, property => property.MapFrom(user => user.Role != null ? user.Role.Name : null));
            Mapper.CreateMap<UserView, User>()
                .ForMember(user => user.DateOfBirth, property => property.MapFrom(view => view.UserDateOfBirth))
                .ForMember(user => user.FirstName, property => property.MapFrom(view => view.UserFirstName))
                .ForMember(user => user.LastName, property => property.MapFrom(view => view.UserLastName))
                .ForMember(user => user.RoleId, property => property.MapFrom(view => view.UserRoleId))
                .AfterMap((view, user) => {
                    if (view.UserRoleId != null)
                    {
                        user.Role = new Role();
                        user.Role.Id = view.UserRoleId;
                        user.Role.Name = view.UserRoleName;
                    }
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
