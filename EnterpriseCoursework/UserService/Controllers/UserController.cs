using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Database;

namespace UserService.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : Controller
    {
        private UserContext db;
        private readonly UserManager<IdentityUser> userManager;

        public UserController(UserContext db, UserManager<IdentityUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }

        // GET api/<InventoryController>/email
        [HttpGet("{email}")]
        public async Task<ActionResult<IdentityUser>> Get(string email)
        {
            IdentityUser user = await userManager.FindByEmailAsync(email);

            if (user != null)
            {
                return Ok(user);
            }

            return NotFound();
        }

        // POST api/<InventoryController>/password
        [HttpPost("{password}")]
        public async Task<ActionResult<IdentityUser>> Post([FromBody] IdentityUser user, string password)
        {
            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Normal");
                IdentityUser identityUser = await userManager.FindByEmailAsync(user.Email);

                return Ok(identityUser);
            }
            string errors = "";
            foreach (var error in result.Errors)
            {
                errors += error.Description;
            }
            return Ok(password);
        }

        // PUT api/<InventoryController>/
        [HttpPut]
        public ActionResult Put([FromBody] IdentityUser user)
        {
            userManager.UpdateAsync(user);

            return NoContent();
        }
    }
}
