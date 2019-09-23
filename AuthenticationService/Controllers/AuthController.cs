using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthenticationService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // GET: api/Auth
        /// <summary>
        /// Use Get to Verify the JWT Token
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get([FromHeader] string Authorization)
        {
            try
            {
                string token = "";
                // Check if Authorization header is in the request
                if (string.IsNullOrEmpty(Authorization))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Can't find Authorization Header");
                }

                // Check if it is using bearer authentication
                if (!Authorization.ToLower().Contains("bearer") || Authorization.ToLower().Substring(0, 7) != "bearer ")
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "No bearer Auth is found in Authroization header");
                }

                // Retrieving token information
                token = Authorization.Substring("bearer ".Length, Authorization.Length - "bearer ".Length);

                if (!(new JWTService()).VerifyToken(token))
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, "Failed to authenticate user, token is not valid");
                }
                return StatusCode(StatusCodes.Status200OK, "Authenticated");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Use POST to Login and get JWT token
        /// </summary>
        /// <param name="value"></param>
        // POST: api/Auth
        [HttpPost]
        public IActionResult Post([FromHeader] string Authorization)
        {
            try
            {
                string username, password = "";
                // Check if Authorization header is in the request
                if(string.IsNullOrEmpty(Authorization))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Can't find Authorization Header");
                }

                // Check if it is using basic authentication
                if (!Authorization.ToLower().Contains("basic") || Authorization.ToLower().Substring(0, 6) != "basic ")
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "No basic Auth is found in Authroization header");
                }

                // Retrieving credential information, credential is Base64 encoded
                string credentials = Encoding.UTF8.GetString(Convert.FromBase64String(Authorization.Substring("basic ".Length, Authorization.Length - "basic ".Length)));
                if (!credentials.Contains(":"))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Can't retrieve username and password from input argument");
                }

                // Get Credentials
                (username, password) = (credentials.Split(':')[0], credentials.Split(':')[1]);

                // Authenticate User against your database/backend
                if(!(new UserService()).AuthenticateUser(username, password))
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, "Can't Authenticate User, Invalid username and/or password");
                }

                // Create token for this authenticated user
                string token = (new JWTService()).CreateToken(username); // Create JWT token

                return Ok(token); // return the token
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
