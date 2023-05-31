namespace SmartdustApi.DTO
{
    public class ChangePasswordDTO
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public int UserId { get; set; }
        public string Username{ get; set; }
    }
}
