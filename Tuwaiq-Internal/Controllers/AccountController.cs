using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Client.AspNetCore;
using OpenIddict.Abstractions;

namespace TuwaiqRecruitment.Controllers;

public class AccountController : Controller
{
    [AllowAnonymous]
    [HttpGet("~/signin")]
    public IActionResult SignIn(string returnUrl = "/")
    {
        var properties = new AuthenticationProperties { RedirectUri = returnUrl };
        return Challenge(properties, OpenIdConnectDefaults.AuthenticationScheme);
    }

    [AllowAnonymous]
    [HttpGet("~/signin-oidc"), IgnoreAntiforgeryToken]
    public async Task<IActionResult> SignInCallback()
    {
        Console.WriteLine("Start");

        var result = await HttpContext.AuthenticateAsync(OpenIdConnectDefaults.AuthenticationScheme);

        // if (!authResult.Succeeded)
        // {
        //     // handle authentication failure
        // }
        //
        // // save tokens for future use
        // var accessToken = authResult.Properties.GetTokenValue("access_token");
        // var refreshToken = authResult.Properties.GetTokenValue("refresh_token");
        //
        // // handle successful authentication
        // return RedirectToAction("Index", "Home");
        
        
        if (result.Principal.Identity is not ClaimsIdentity { IsAuthenticated: true })
        {
            throw new InvalidOperationException("The external authorization data cannot be used for authentication.");
        }

        var claims = new List<Claim>(result.Principal.Claims
            .Select(claim => claim switch
            {
                { Type: OpenIddictConstants.Claims.Subject }
                    => new Claim(ClaimTypes.NameIdentifier, claim.Value, claim.ValueType, claim.Issuer),

                { Type: OpenIddictConstants.Claims.Name }
                    => new Claim(ClaimTypes.Name, claim.Value, claim.ValueType, claim.Issuer),

                _ => claim
            })
           
            )
            ;

        var identity = new ClaimsIdentity(claims,
            authenticationType: CookieAuthenticationDefaults.AuthenticationScheme,
            nameType: ClaimTypes.Name,
            roleType: ClaimTypes.Role);

        var properties = new AuthenticationProperties(result.Properties.Items);

        properties.StoreTokens(result.Properties.GetTokens().Where(token => token switch
        {
            {
                Name: OpenIddictClientAspNetCoreConstants.Tokens.BackchannelAccessToken   or
                      OpenIddictClientAspNetCoreConstants.Tokens.BackchannelIdentityToken or
                      OpenIddictClientAspNetCoreConstants.Tokens.RefreshToken
            } => true,

            // Ignore the other tokens.
            _ => false
        }));
         SignIn(new ClaimsPrincipal(identity), properties, CookieAuthenticationDefaults.AuthenticationScheme);
         Console.WriteLine(properties.RedirectUri);
         return Redirect(properties.RedirectUri);
         
    }

    [HttpGet("~/signout")]
    public async Task<IActionResult> SignOut()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);

        await HttpContext.SignOutAsync();

        return SignOut(
            authenticationSchemes: CookieAuthenticationDefaults.AuthenticationScheme,//OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
            properties: new AuthenticationProperties
            {
                RedirectUri = "/"
            });
    }
}

