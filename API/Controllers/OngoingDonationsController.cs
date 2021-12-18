using Business.Abstract;
using Entities.Concrete;
using Entities.DTOs.Donation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OngoingDonationsController : ControllerBase
    {
        private readonly IUserOngoingDonationService _ongoingDonationService;

        public OngoingDonationsController(IUserOngoingDonationService ongoingDonationService)
        {
            _ongoingDonationService = ongoingDonationService;
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] OngoingDonationDto userOngoingDonation)
        {
            return Ok(await _ongoingDonationService.InsertUserDonation(userOngoingDonation));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery, Required] int userId)
        {
            return Ok(await _ongoingDonationService.GetUserDonations(userId));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            return Ok(await _ongoingDonationService.DeleteUserDonation(id));
        }
    }
}
