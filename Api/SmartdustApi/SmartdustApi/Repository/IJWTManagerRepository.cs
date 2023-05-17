using SmartdustApi.Models;

namespace SmartdustApi.Repository
{

    public interface IJWTManagerRepository
    {
        Tokens Authenticate(Users users);
    }

}
