using System.ComponentModel.DataAnnotations;

namespace SmartdustApi.Models
{
    public class LeaveDTO
    {
        public int UserID { get; set; }
        public DateTime LeaveFrom { get; set; }
        public DateTime LeaveTill { get; set; }
        public string Reason { get; set; }
        public DateTime AppliedDate { get; set; }
        public string LeaveStatus { get; set; }

        public int LeaveDays { get; set; }
    }
}
