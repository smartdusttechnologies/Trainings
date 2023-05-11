using SmartdustApi.Common;
using SmartdustApi.Models;

namespace SmartdustApi.Services.Interfaces
{
    public interface IContactService
    {
        RequestResult<bool> Save(ContactDTO contact);
    }
}
