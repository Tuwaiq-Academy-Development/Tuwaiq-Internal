// using System.Security.Claims;
// using Microsoft.AspNetCore.Authentication.Cookies;
// using OpenIddict.Abstractions;
// using Internal.Data;
//
// namespace AdminUi.Helper;
//
// public class AddCompanyClaimsMiddleware
// {
//     private readonly RequestDelegate _next;
//
//     public AddCompanyClaimsMiddleware(RequestDelegate next)
//     {
//         _next = next;
//     }
//
//     public async Task Invoke(HttpContext context)
//     {
//         if (context.User.Identity?.IsAuthenticated == true)
//         {
//             var claimsIdentity = context.User.Identity as ClaimsIdentity;
//
//             if (claimsIdentity != null)
//             {
//                 var claims = claimsIdentity.Claims.ToList();
//                 if (!claims.Any(c => c.Type == "company_id"))
//                 {
//                     var dbContext = context.RequestServices.GetRequiredService<ApplicationDbContext>();
//
//                     var userIdClaim = claims.FirstOrDefault(c => c.Type == OpenIddictConstants.Claims.Subject)?.Value;
//                     Guid.TryParse(userIdClaim, out var companyId);
//                     var dbContextCompanies = dbContext.Companies.AsEnumerable()
//                         .FirstOrDefault(c => c.Users != null && c.Users.Contains(companyId))!;
//                     var company = dbContextCompanies;
//                     if (company != null)
//                     {
//                         claims.Add(new Claim("company_id", company.Id.ToString()));
//                         claims.Add(new Claim("company_name", company.Name));
//                         if (!string.IsNullOrEmpty(company.Logo))
//                         {
//                             var logo = "/Storage/" + company?.Logo;
//                             claims.Add(new Claim("company_logo", logo));
//                         }
//                     }
//
//                     var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
//                     context.User = new ClaimsPrincipal(identity);
//                     SignIn(new ClaimsPrincipal(identity), properties, CookieAuthenticationDefaults.AuthenticationScheme)
//                 }
//             }
//         }
//
//         await _next(context);
//     }
// }