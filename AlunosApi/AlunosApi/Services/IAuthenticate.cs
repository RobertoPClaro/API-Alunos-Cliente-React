namespace AlunosApi.Services
{
    public interface IAuthenticate
    {
        Task<bool> Authenticate(string email, string password); //faz o login
        Task<bool> RegisterUser(string email, string password); //registra o usuario
        Task Logout();
    }
}
