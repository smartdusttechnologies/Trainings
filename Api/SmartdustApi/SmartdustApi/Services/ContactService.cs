using SmartdustApi.Common;
using SmartdustApi.Model;
using SmartdustApi.Repository.Interfaces;
using SmartdustApi.Services.Interfaces;
using TestingAndCalibrationLabs.Business.Core.Interfaces;
using TestingAndCalibrationLabs.Business.Core.Model;

namespace SmartdustApi.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;
        private readonly IEmailService _emailService;

        public ContactService(IContactRepository contactRepository, IEmailService emailservice)
        {
            _contactRepository = contactRepository;
            _emailService = emailservice;
        }

        public RequestResult<bool> Save(ContactDTO contact)
        {
            
            EmailModel model = new EmailModel();
            model.Name = contact.Name;
            model.Mail = contact.Mail;
            model.Phone = contact.Phone;
            model.Subject = contact.Subject;
            model.Address = contact.Address;
            model.Message = contact.Message;
            //model.Email = "yashrajsmartdust@gmail.com";
            //model.Email.RemoveAt(0);
            model.Email = "yashrajsmartdust@gmail.com";
            var isemailsendsuccessfully = _emailService.Sendemail(model);

            var result = _contactRepository.Save(contact);
            if (isemailsendsuccessfully)
            {
                List<ValidationMessage> success = new List<ValidationMessage>()
                {
                    new ValidationMessage(){Reason = "Thank You We Will Contact You As soon As Possible",Severity=ValidationSeverity.Information}
                };
                result.Message = success;
                return result;
            }
            List<ValidationMessage> error = new List<ValidationMessage>()
                {
                    new ValidationMessage(){Reason = "Unable To take Your Request Right Now",Severity=ValidationSeverity.Information}
                };
            result.Message = error;
            return result;
        }
    }
}
