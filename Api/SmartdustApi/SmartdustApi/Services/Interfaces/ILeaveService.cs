using SmartdustApi.Common;
using SmartdustApi.Models;

namespace SmartdustApi.Services.Interfaces
{
    public interface ILeaveService
    {
        RequestResult<List<LeaveDTO>> Get();
    }
}
