using SmartdustApi.Common;
using SmartdustApi.Models;

namespace SmartdustApi.Repository.Interfaces
{
    public interface IContactRepository
    {
        RequestResult<bool> Save(ContactDTO contact);
    }
}
