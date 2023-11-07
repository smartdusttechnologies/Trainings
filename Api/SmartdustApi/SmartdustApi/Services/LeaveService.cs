using SmartdustApi.Common;
using SmartdustApi.Models;
using SmartdustApi.Repository.Interfaces;
using SmartdustApi.Services.Interfaces;

namespace SmartdustApi.Services
{
    public class LeaveService : ILeaveService
    {
        private readonly ILeaveRepository _leaveRepository;

        public LeaveService(ILeaveRepository leaveRepository)
        {
            _leaveRepository = leaveRepository;
        }

        public RequestResult<List<LeaveDTO>> Get()
        {
            var leave = _leaveRepository.Get();
            if (leave == null)
            {
                return new RequestResult<List<LeaveDTO>>();
            }
            return new RequestResult<List<LeaveDTO>>(leave);
        }
    }
}
