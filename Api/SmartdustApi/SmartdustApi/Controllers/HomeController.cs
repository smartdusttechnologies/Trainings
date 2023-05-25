using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartdustApi.Common;
using SmartdustApi.Model;
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
        private readonly IOrganizationService _organizationService;

        public HomeController(ILogger<HomeController> logger, IContactService contactService,IOrganizationService organizationService)
        {
            _logger = logger;
            _contactService = contactService;
            _organizationService = organizationService;
        }

        [HttpPost]
        [Route("Contactus")]
        public RequestResult<bool> Contactsus(ContactDTO contact)
        {
            return _contactService.Save(contact);
        }
        
        [HttpGet]
        [Route("GetOrganizations")]
        public RequestResult<List<OrganizationModel>> GetOrganizations()
        {
            return _organizationService.Get();
        }
    }
}