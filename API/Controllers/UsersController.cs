using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Business.Concrete;
using DataAccess.Concrete;
using Entities.DTOs.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] InsertUserDto insertUserDto)
        {
            return Ok(await _userService.RegisterUser(insertUserDto));
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            return Ok(await _userService.Login(loginDto));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserDetail([FromRoute, Required] int id)
        {
            return Ok(await _userService.GetUserDetail(id));
        }
    }
}
