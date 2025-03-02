using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookReviewApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace BookReviewApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] //Only Admin users can access controller actions
    public class RolesController : ControllerBase
    {
        //Services that manage roles and users
        private readonly RoleManager<IdentityRole> _roleManager; 
        private readonly UserManager<IdentityUser> _userManager; 

        //Injecting dependencies to create, delete, and manage roles and users 
        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager) //
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet] // Method to retrieve all roles 
        public IActionResult GetRoles() 
        {
            var roles = _roleManager.Roles.ToList(); // Retrieves roles form role manager 
            return Ok(roles); // Returns list of roles 
        }

        [HttpGet("{roleId}")] // HTTP method to get role by id 
        public async Task<IActionResult> GetRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId); //Finds the role by id 

            if (role == null) //If role not found a reponse is returned
            {
                return NotFound("Role not found.");
            }

            return Ok(role);//If found, role of specific id is returned 
        }

        [HttpPost] //HTTP method to create a role 
        public async Task<IActionResult> CreateRole([FromBody] string roleName)
        {
            var role = new IdentityRole(roleName); // Creates role using role name 
            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded) // if role was created return message is returned
            {
                return Ok("Role created successfully.");
            }

            return BadRequest(result.Errors); //Else bad request is returned with errors
        }

        [HttpPut] //HTTP Method to update a role 
        public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.RoleId); //Find role by id

            if (role == null) //if role not found a not found response is returned 
            {
                return NotFound("Role not found.");
            }

            role.Name = model.NewRoleName; //Else the role name update is attempted
            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded) // If role is successfully updated a message is returned
            {
                return Ok("Role updated successfully.");
            }

            return BadRequest(result.Errors);// Else update is unsuccessful and bad request is returned with errors
        }

        [HttpDelete] //HTTP method to delete a role 
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);//Finds role by id

            if (role == null) ////if role not found a not found response is returned 
            {
                return NotFound("Role not found.");
            }

            var result = await _roleManager.DeleteAsync(role); //Attemps to delete role

            if (result.Succeeded) //If deletion successful, success message is returned
            {
                return Ok("Role deleted successfully.");
            }

            return BadRequest(result.Errors); // Else deletion is unsuccessful and bad request is returned with errors
        }

        [HttpPost("assign-role-to-user")] //HTTP method to assign a role to a user 
        public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId); //Finds user by id

            if (user == null) //if user not found, not found is returned 
            {
                return NotFound("User not found.");
            }

            var roleExists = await _roleManager.RoleExistsAsync(model.RoleName); //Checks if role exists 

            if (!roleExists) //If role does not exist not found reponse is returned
            {
                return NotFound("Role not found.");
            }

            var result = await _userManager.AddToRoleAsync(user, model.RoleName); //Attempt to assign role to user 

            if (result.Succeeded) //if successful, OK repsonse is returned 
            {
                return Ok("Role assigned to user successfully.");
            }

            return BadRequest(result.Errors); // Else unsuccessful and bad request is returned with errors
        }

    }
}
