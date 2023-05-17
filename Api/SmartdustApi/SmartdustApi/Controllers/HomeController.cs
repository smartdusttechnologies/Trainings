using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartdustApi.Common;
using SmartdustApi.Models;
using SmartdustApi.Repository;
using SmartdustApi.Services.Interfaces;

namespace SmartdustApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IContactService _contactService;
        private readonly IJWTManagerRepository _jWTManager;
        public HomeController(ILogger<HomeController> logger, IContactService contactService, IJWTManagerRepository jWTManager)
        {
            _logger = logger;
            _contactService = contactService;
            _jWTManager = jWTManager;
        }

        [HttpPost(Name = "Contactus")]
        public RequestResult<bool> Contactsus(ContactDTO contact)
        {
            return _contactService.Save(contact);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("authenticate")]
        public IActionResult Authenticate(Users usersdata)
        {
            var token = _jWTManager.Authenticate(usersdata);

            if (token == null)
            {
                return Unauthorized();
            }

            return Ok(token);
        }
        [Authorize]
        [HttpGet]
        public List<string> Get()
        {
            var users = new List<string>
        {
            "Satinder Singh",
            "Amit Sarna",
            "Davin Jon"
        };

            return users;
        }

    }
}