namespace ProfileApi.Services
{
    public interface ILoginService
    {
        string Login(string username, string password);
    }
}