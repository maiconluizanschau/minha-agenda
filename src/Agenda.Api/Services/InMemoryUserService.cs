namespace Agenda.Api.Services;

public class InMemoryUserService : IUserService
{
    // Usuário fake apenas para teste
    public bool ValidateUser(string username, string password)
        => username == "admin" && password == "admin123";
}
