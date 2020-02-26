using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces.Definition;
using Domain.Common.Extensions;
using Domain.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HubbecAPI.Controllers
{
  [AllowAnonymous]
  [Route("api/[controller]")]
  [ApiController]
  public class AccountController : ControllerBase
  {
    private readonly IUserService _userService;

    public AccountController(IUserService userService)
    {
      _userService = userService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody]Login model)
    {
      var user = _userService.Authenticate(model.Email, model.Password);

      if (user == null)
        return BadRequest(new { message = "Username or password is incorrect" });

      return Ok(user);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody]User model)
    {
      model.Password = model.Password.EncryptMD5();
      var user = await _userService.Create(model);
      if (!user.Status)
        return BadRequest(user);

      return Created(Request.Path, user);
    }

    [HttpGet("check-login")]
    public IActionResult CheckLogin()
    {
      if (User.Identity.IsAuthenticated)
        return Ok();

      return BadRequest();
    }

  }
}