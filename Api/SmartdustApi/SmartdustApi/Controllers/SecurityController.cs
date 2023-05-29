using SmartdustApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SmartdustApi.Common;
using SmartdustApi.Model;
using SmartdustApi.Model;
using SmartdustApi.DTO;

namespace SmartdustApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SecurityController : Controller
    {
        private readonly IAuthenticationService _authenticationService;

        public SecurityController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        [HttpPost]
        [Route("SignUp")]
        public IActionResult SignUp(UserDTO user)
        {
            var userModel  = new UserModel()
            {
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                OrgId = user.OrgId,
                Mobile = user.Mobile,
                Country = user.Country,
                NewPassword = user.NewPassword,
                Password = user.Password
            };
            RequestResult<bool> result = _authenticationService.Add(userModel);
            if (result.IsSuccessful)
            {
                return Json(new { status = true, message = "Account Created Successfull!" });
            }
            return Json(result);
        }
        /// <summary>
        /// Method to get the Login details from UI and Process Login.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginDTO loginRequest)
        {
            var loginReq = new LoginRequest { UserName = loginRequest.UserName, Password = loginRequest.Password };
            RequestResult<LoginToken> result = _authenticationService.Login(loginReq);
            if (result.IsSuccessful)
            {
                return Json(result);
            }
            return Json(result);
        }
        [HttpPost]
        [Route("ChangePassword")]
        public IActionResult ChangePassword(ChangePasswordDTO changepasswordDTO)
        {
            var changepasswordRequest = new ChangePasswordModel()
            {
                OldPassword = changepasswordDTO.OldPassword,
                NewPassword = changepasswordDTO.NewPassword,
                ConfirmPassword = changepasswordDTO.ConfirmPassword
            };
            var user = HttpContext.User.Identities.FirstOrDefault() as SdtUserIdentity;
            if (user != null)
            {
                changepasswordRequest.UserId = user.UserId;
                changepasswordRequest.Username = user.UserName;
                var result = _authenticationService.UpdatePaasword(changepasswordRequest);
                if (result.IsSuccessful)
                {
                    return Ok(result.RequestedObject);
                }
                return BadRequest(result.ValidationMessages);
            }
            return BadRequest(new RequestResult<bool>(false));
        }
    }
}
