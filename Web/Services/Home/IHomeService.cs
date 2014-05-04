namespace Template.Services
{
    public interface IHomeService : IService
    {
        void AddPageNotFoundMessage();
        void AddSystemErrorMessage();
        void AddUnauthorizedMessage();
    }
}
