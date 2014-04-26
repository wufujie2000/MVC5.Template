namespace Template.Components.Security
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
            Instance = instance; // TODO: Remove all null checks on role provider
        }
    }
}
