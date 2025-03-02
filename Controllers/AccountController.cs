using BookReviewApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace BookReviewApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager; // Manages user-related operations
        private readonly SignInManager<IdentityUser> _signInManager; // Manages user login and sign-out
        private readonly EmailService _emailService; // Email Service 
        private readonly IConfiguration _configuration; // Accesses configuration - JWT settings

        //Injecting Dependencies 
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, EmailService emailService, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _configuration = configuration;
        }

        [HttpPost("register")] //Endpoint to register user 
        public async Task<IActionResult> Register(AuthModel model)
        {
            // Create a new identity user with email and password
            var user = new IdentityUser { UserName = model.Email, Email = model.Email }; 
            var result = await _userManager.CreateAsync(user, model.Password); 

            if (result.Succeeded) //If creation is success
            {
                // Generate an email verification token
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                // Create the verification link
                var verificationLink = Url.Action("VerifyEmail", "Account", new { userId = user.Id, token = token }, Request.Scheme);

                // Send the verification email
                var emailSubject = "Email Verification";
                var emailBody = $"Please verify your email by clicking the following link: {verificationLink}";
                _emailService.SendEmail(user.Email, emailSubject, emailBody);
               
                return Ok("User registered successfully. An email verification link has been sent."); 
            }

            return BadRequest(result.Errors); //If failure, bad request response is returned
        }


        // Add an action to handle email verification
        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId); //Find user by id

            if (user == null) //If user not found reponse is returned 
            {
                return NotFound("User not found.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token); //User confrimed with token 

            if (result.Succeeded) //If verification successful, response is returned 
            {
                return Ok("Email verification successful.");
            }

            return BadRequest("Email verification failed."); // Failure - bad requestreponse 
        }



        [HttpPost("login")] //Endpoint to login
        public async Task<IActionResult> Login(AuthModel model)
        {
            //Sgin in the user using email and password
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded) //If login successful, email and role is retrieved 
            {
                var user = await _userManager.FindByEmailAsync(model.Email); 
                var roles = await _userManager.GetRolesAsync(user);
                var token = GenerateJwtToken(user,roles); //Token is generated and returned
                return Ok(new { Token = token });
            }

            return Unauthorized("Invalid login attempt."); //Else return unauthorized response 
        }

        [HttpPost("logout")] //Endpoint for user to logout
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync(); // User is signed out
            return Ok("Logged out"); //Ok reponse returned 
        }
        private string GenerateJwtToken(IdentityUser user, IList<string> roles) //Helper method to generate a token 
        {
            var claims = new List<Claim>
            {
                //Add user email as claim and Unique id for JWT
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            // Add roles as claims - important for authorization
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])); // Signing key created 
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); // Signing credentials created - ensures cannot be modified 
            var expires = DateTime.Now.AddHours(Convert.ToDouble(_configuration["Jwt:ExpireHours"])); // Setting expiration time

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: expires, //set expiration
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token); //Return JWT Token
        }

    }

}