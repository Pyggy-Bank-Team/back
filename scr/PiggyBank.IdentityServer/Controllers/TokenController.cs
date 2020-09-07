using System;
using Microsoft.AspNetCore.Mvc;
using PiggyBank.IdentityServer.Extensions;

namespace PiggyBank.IdentityServer.Controllers
{
    [ApiController, Route("[controller]")]
    public class TokenController : ControllerBase
    {
        [HttpGet, Route("CheckValid")]
        public IActionResult CheckToken()
        {
            var token = Request.GetToken();

            if (token == null)
            {
                var errorResponse = new
                {
                    code = "TokenIsNullOrEmpty",
                    description = "Bearer token not found"
                };
                return BadRequest(errorResponse);
            }

            var validTo = token.ValidTo;
            var response = new
            {
                IsValid = (validTo - DateTime.UtcNow).Hours > 8,
                validTo
            };

            return Ok(response);
        }
    }
}