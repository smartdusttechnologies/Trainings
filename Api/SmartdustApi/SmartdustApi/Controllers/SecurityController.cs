using SmartdustApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SmartdustApi.Common;
using SmartdustApi.Model;
using SmartdustApi.Model;

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
            //_orgnizationService = orgnizationService;
            //_mapper = mapper;
        }

        /// <summary>
        /// UI Shows the Orgnizations names in dropdown list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            // List<Business.Core.Model.Organization> organizations = _orgnizationService.Get();
            //List<SelectListItem> organizationNames = organizations.Select(x => new SelectListItem { Text = x.OrgName, Value = x.Id.ToString() }).ToList();
            //ViewBag.Organizations = organizationNames;
            return View();
        }
        [HttpPost]
        [Route("SignUp")]
        public IActionResult SignUp(UserModel user, string password)
        {

            RequestResult<bool> result = _authenticationService.Add(user, password);
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
        public IActionResult ChangePassword(ChangePasswordModel changepasswordRequest)
        {

            //var psswReq = new ChangePasswordModel { OldPassword = changepasswordRequest.OldPassword, NewPassword = changepasswordRequest.NewPassword, ConfirmPassword = changepasswordRequest.ConfirmPassword, UserId = changepasswordRequest.UserId };
            var result = _authenticationService.UpdatePaasword(changepasswordRequest);
            if (result.IsSuccessful)
            {
                return Ok(result.RequestedObject);
            }
            return BadRequest(result.ValidationMessages);
        }
    }
}
