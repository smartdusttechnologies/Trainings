using System.Security.Claims;

namespace Shopping.Web.Pages.Account
{
     public class ProfileModel : PageModel
     {
          public string? UserName { get; set; }
          public string? UserEmailAddress { get; set; }
          public string? UserProfileImage { get; set; }
          public string? UserPhoneNumber { get; set; }
          public string? UserAddress { get; set; }

          public void OnGet()
          {
               UserName = User.Identity.Name;
               UserEmailAddress = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
               UserProfileImage = User.FindFirst(c => c.Type == "picture")?.Value;
               UserPhoneNumber = User.FindFirst(c => c.Type == ClaimTypes.MobilePhone)?.Value;
               UserAddress = User.FindFirst(c => c.Type == ClaimTypes.StreetAddress)?.Value;
          }
     }
}
