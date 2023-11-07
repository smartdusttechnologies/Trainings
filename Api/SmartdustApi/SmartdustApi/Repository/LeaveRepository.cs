using Dapper;
using SmartdustApi.Infrastructure;
using SmartdustApi.Models;
using SmartdustApi.Repository.Interfaces;
using System.Data;

namespace SmartdustApi.Repository
{
    public class LeaveRepository : ILeaveRepository
    {

        private readonly IConnectionFactory _connectionFactory;

        public LeaveRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public List<LeaveDTO> Get()
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Query<LeaveDTO>("select * from [Leave] WHERE UserID = 4").ToList();
        }
    }
}
