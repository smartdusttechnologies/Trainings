﻿using SmartdustApi.Common;
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
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;


        public ContactService(IContactRepository contactRepository, IEmailService emailservice, IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            _contactRepository = contactRepository;
            _emailService = emailservice;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        public RequestResult<bool> Save(ContactDTO contact)
        {
            
            EmailModel model = new EmailModel();

            //Read other values from Appsetting .Json 
            model.EmailTemplate = _configuration["TestingAndCalibrationSurvey:UserTemplate"];
            model.Subject = _configuration["TestingAndCalibrationSurvey:Subject"];

            //Create User Mail
            model.HtmlMsg = CreateBody(model.EmailTemplate);
            model.HtmlMsg = model.HtmlMsg.Replace("*name*", contact.Name);
            model.HtmlMsg = model.HtmlMsg.Replace("*Emailid*", contact.Mail);
            //model.HtmlMsg = model.HtmlMsg.Replace("*mobilenumber*", contact.Phone);
            model.HtmlMsg = model.HtmlMsg.Replace("*Subject*", contact.Subject);
            model.HtmlMsg = model.HtmlMsg.Replace("*Address*", contact.Address);
            model.HtmlMsg = model.HtmlMsg.Replace("*Message*", contact.Message);
            model.Subject = model.Subject;

            model.Email = new List<string>();
            model.Email.Add("yashrajsmartdust@gmail.com");

            var isemailsendsuccessfully = _emailService.Sendemail(model);

            var result = _contactRepository.Save(contact);
            if (isemailsendsuccessfully.RequestedObject)
            {
                List<ValidationMessage> success = new List<ValidationMessage>()
                {
                    new ValidationMessage(){Reason = "Thank You We Will Contact You As soon As Possible",Severity=ValidationSeverity.Information}
                };
                isemailsendsuccessfully.Message = success;
                return isemailsendsuccessfully;
            }
            List<ValidationMessage> error = new List<ValidationMessage>()
                {
                    new ValidationMessage(){Reason = "Unable To take Your Request Right Now",Severity=ValidationSeverity.Information}
                };
            isemailsendsuccessfully.Message = error;
            return isemailsendsuccessfully;
        }

        /// <summary>
        /// To use the email Template to send mail to the User participated.
        /// </summary>
        /// <param name="emailTemplate"></param>
        ///returns></returns>
        private string CreateBody(string emailTemplate)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Path.Combine(_hostingEnvironment.WebRootPath, _configuration["TestingAndCalibrationSurvey:UserTemplate"])))
            {
                body = reader.ReadToEnd();
            }
            return body;
        }
    }
}