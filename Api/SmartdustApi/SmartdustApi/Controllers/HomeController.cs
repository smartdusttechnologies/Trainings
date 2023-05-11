using Microsoft.AspNetCore.Mvc;
using SmartdustApi.Common;
using SmartdustApi.Models;
using SmartdustApi.Services.Interfaces;

namespace SmartdustApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<HomeController> _logger;
        private readonly IContactService _contactService;

        public HomeController(ILogger<HomeController> logger,IContactService contactService)
        {
            _logger = logger;
            _contactService = contactService;
        }

        [HttpPost(Name = "Contactus")]
        public RequestResult<bool> Contactsus(ContactDTO contact)
        {
            return _contactService.Save(contact);
        }
    }
}