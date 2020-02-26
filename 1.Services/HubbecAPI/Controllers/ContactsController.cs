using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces.Definition;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HubbecAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  [Authorize]
  public class ContactsController : ControllerBase
  {
    private readonly IUserService _userService;
    private readonly Guid _userId;

    public ContactsController(IUserService userService)
    {
      _userService = userService;
      _userId = Guid.Parse(User.Identity.Name);
    }

    [HttpGet("")]
    public async Task<IActionResult> Get()
    {
      var user = await _userService.GetUserWithContacts(_userId);
      return Ok(user);
    }

    [HttpPost("add/{contactId}")]
    public async Task<IActionResult> Add(Guid contactId)
    {
      _userService.AddContact(_userId, contactId);
      return Ok();
    }
  }
}