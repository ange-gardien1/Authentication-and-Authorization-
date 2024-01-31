using challegeToDo_Backend.Models;

namespace challegeToDo_Backend.Repositories;

public interface IAuthService
{
    user CreateUser(user user);
    string Login(string email, string password);
    
}