using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace NetcoreSocialLoginExample.Controllers
{
    [AllowAnonymous, Route("account")]
    public class AccountController : Controller
    {
        [Route("google-login")]
        public IActionResult GoogleLogin()
        {

            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse")
            };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [Route("google-reponse")]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var claims = result.Principal.Identities.FirstOrDefault()
                .Claims.Select(claim => new
                {
                    claim.Issuer,
                    claim.OriginalIssuer,
                    claim.Type,
                    claim.Value,
                });

            var providerKey = claims
                .Where(x => x.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"))
                .Select(x => x.Value)
                .FirstOrDefault();

            var emailAddress = claims
                .Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")
                .Select(x => x.Value)
                .FirstOrDefault();

            var name = claims //display name
                .Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")
                .Select(x => x.Value)
                .FirstOrDefault();

            var givenName = claims
                .Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname")
                .Select(x => x.Value)
                .FirstOrDefault();

            var surName = claims
                .Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname")
                .Select(x => x.Value)
                .FirstOrDefault();


            ViewData["ProviderKey"] = providerKey;
            ViewData["EmailAddress"] = emailAddress;
            ViewData["Name"] = name;
            ViewData["GivenName"] = givenName;
            ViewData["SurName"] = surName;

            return View();
        }
    }
}
