namespace Template.Components.Services
{
    public interface IHomeService : IService
    {
        void AddPageNotFoundMessage();
        void AddSystemErrorMessage();
        void AddUnauthorizedMessage();
    }
}
