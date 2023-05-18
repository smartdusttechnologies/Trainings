using SmartdustApi.Common;
using SmartdustApi.Model;
using SmartdustApi.Repository.Interfaces;
using SmartdustApi.Services.Interfaces;

namespace SmartdustApi.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;

        public ContactService(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public RequestResult<bool> Save(ContactDTO contact)
        {
           return _contactRepository.Save(contact);
        }
    }
}
