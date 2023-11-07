using Microsoft.AspNetCore.Mvc;

using SmartdustApi.Common;
using SmartdustApi.Model;
using SmartdustApi.Models;
using SmartdustApi.Services.Interfaces;
using TestingAndCalibrationLabs.Business.Core.Interfaces;

namespace SmartdustApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IContactService _contactService;
        private readonly IOrganizationService _organizationService;

        public HomeController(ILogger<HomeController> logger, IContactService contactService, IOrganizationService organizationService)
        {
            _logger = logger;
            _contactService = contactService;
            _organizationService = organizationService;
        }

        [HttpPost]
        [Route("Contactus")]
        public IActionResult Contactsus(ContactDTO contact)
        {
            var result = _contactService.Save(contact);
            if (result.RequestedObject)
            {
                return Json(result);
            }
            return BadRequest(result);
        }

        [HttpGet]
        [Route("GetOrganizations")]
        public IActionResult GetOrganizations()
        {
            var list = _organizationService.Get();
            if (list.IsSuccessful)
            {
                return Json(list);
            }

            List<ValidationMessage> errors = new List<ValidationMessage>
                {
                    new ValidationMessage { Reason = "Something Went Wrong", Severity = ValidationSeverity.Error, SourceId = "fields" }
                };
            return Json(new RequestResult<bool>(errors));
        }
    }
}