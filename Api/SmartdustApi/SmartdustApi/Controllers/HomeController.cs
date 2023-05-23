using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartdustApi.Common;
using SmartdustApi.Model;
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

        public HomeController(ILogger<HomeController> logger, IContactService contactService)
        {
            _logger = logger;
            _contactService = contactService;
        }

        [HttpPost]
        [Route("Contactus")]
        public RequestResult<bool> Contactsus(ContactDTO contact)
        {
            return _contactService.Save(contact);
        }
        
        [HttpGet]
        [Route("GetName")]
        public string GetName()
        {
            return "Raj";
        }
    }
}