using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhoneBook.Web.Models;
using PhoneBook.Web.Repository;

namespace PhoneBook.Web.Controllers
{
    [Route("api/[controller]")]
    public class PhoneBookController : Controller
    {
        private readonly IPhoneBookRepository _phoneBookRepository;

        /// <inheritdoc />
        public PhoneBookController(IPhoneBookRepository phoneBookRepository)
        {
            this._phoneBookRepository = phoneBookRepository;
        }

        [HttpGet("[action]")]
        public IList<PhoneBookContact> Get()
        {
            return _phoneBookRepository.GetAllContacts();
        }

        [HttpPost("[action]")]
        public IActionResult Post([FromBody] PhoneBookContactDto phoneBookContactDto)
        {
            //Check if contact exists then update, otherwise insert new.
            bool result;
            if (_phoneBookRepository.CheckifContactExists(phoneBookContactDto.Name))
            {
                //Check if the entry already exists and return 409 if true
                if(_phoneBookRepository.CheckifContactEntryExists(phoneBookContactDto))
                    return StatusCode(StatusCodes.Status409Conflict);
                result = _phoneBookRepository.UpdateContact(phoneBookContactDto);
            }
            else
                result = _phoneBookRepository.InsertContact(phoneBookContactDto);
                

            if (!result)
                return StatusCode(StatusCodes.Status500InternalServerError);

            return Ok();
        }

        [HttpDelete("[action]")]
        public IActionResult Delete([FromBody] PhoneBookContactDto phoneBookContactDto)
        {
            bool result = _phoneBookRepository.DeleteContact(phoneBookContactDto);

            if (!result)
                return StatusCode(StatusCodes.Status500InternalServerError);

            return Ok();
        }
    }
}
