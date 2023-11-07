using SmartdustApi.Models;

namespace SmartdustApi.Repository.Interfaces
{
    public interface ILeaveRepository
    {
        List<LeaveDTO> Get();
    }
}
