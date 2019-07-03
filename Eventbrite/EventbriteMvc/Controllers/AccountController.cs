using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace EventbriteMvc.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        [Authorize]

        public async Task<IActionResult> SignIn (string returnUrl)
        {
            var user = User as ClaimsPrincipal;

            var token = await HttpContext.GetTokenAsync("access_token");
            var idToken = await HttpContext.GetTokenAsync("id_token");
            foreach (var claim in user.Claims)
            {
                Debug.WriteLine($"Claim Type: {claim.Type} - Claim Value: {claim.Value}");

            }

            if(token != null)
            {
                ViewData["access_token"] = token;
            }

            if (idToken != null)
            {
                ViewData["id_token"] = idToken;
            }
            // "Event" because UrlHelper doesn't support nameof() for controllers
            // https://github.com/aspnet/Mvc/issues/5853
            return RedirectToAction(nameof(EventController.About), "Event");
        }

        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);

            var homeUrl = Url.Action(nameof(EventController.Index), "Event");
            return new SignOutResult(OpenIdConnectDefaults.AuthenticationScheme, new
                AuthenticationProperties
            { RedirectUri = homeUrl });
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}