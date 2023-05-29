namespace SmartdustApi.DTO
{
    public class UserDTO
    {
        /// <summary>
        /// User Name.
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// First Name.
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Last Name.
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Email Address.
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Mobile.
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// Country.
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// IsdCode.
        /// </summary>
        public string ISDCode { get; set; }
        public int MobileValidationStatus { get; set; }
        public int OrgId { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }

    }
}
