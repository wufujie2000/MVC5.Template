namespace Template.Components.Security.Authorization
{
    public sealed class RoleProviderFactory
    {
        public static IRoleProvider Instance
        {
            get;
            private set;
        }

        public static void SetInstance(IRoleProvider instance)
        {
            Instance = instance;
        }
    }
}
