namespace Agenda.Api.Services;

public interface IUserService
{
    bool ValidateUser(string username, string password);
}
