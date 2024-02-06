using System.Globalization;
using System.Security.Claims;
using IdentityService.SDK;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using TuwaiqInternal.Data;
using TuwaiqInternal.Helper;
using Vite.AspNetCore.Extensions;


var supportedCultures = new[] { new CultureInfo("ar-EG") };

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

// builder.Services.AddViteServices();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options
        .UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion("8.0.33"), x =>
            x.CommandTimeout(180).EnableRetryOnFailure())
);

builder.Services.AddControllersWithViews()
#if DEBUG
    .AddRazorRuntimeCompilation()
#endif
    ;

var ssoSettings = new SSOSettings();

builder.Configuration.GetSection(nameof(SSOSettings)).Bind(ssoSettings);

builder.Services.AddOptions<SSOSettings>()
    .BindConfiguration(nameof(SSOSettings))
    .ValidateOnStart()
    ;


TuwaiqIdentityServicesApiSettings tuwaiqIdentityServicesApiSettings = new();
builder.Configuration.GetSection(nameof(TuwaiqIdentityServicesApiSettings))
    .Bind(tuwaiqIdentityServicesApiSettings);

builder.Services.AddOptions<TuwaiqIdentityServicesApiSettings>()
    .BindConfiguration(nameof(TuwaiqIdentityServicesApiSettings))
    .ValidateOnStart();
builder.Services.SetupIdentityServiceApi(tuwaiqIdentityServicesApiSettings);


builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var cultureInfo = supportedCultures.FirstOrDefault();
    options.DefaultRequestCulture = new RequestCulture(cultureInfo!);
    var optionsSupportedCultures = supportedCultures.Select(s => s).ToList();
    options.SupportedCultures = optionsSupportedCultures;
    options.SupportedUICultures = optionsSupportedCultures;
});

IdentityModelEventSource.ShowPII = true;
builder.Services.AddAccessTokenManagement();
builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddCookie(options =>
    {
        options.LoginPath = "/signin";
        options.LogoutPath = "/signout";
        options.Events.OnSigningOut = async e => { await e.HttpContext.RevokeUserRefreshTokenAsync(); };

        options.CookieManager = new ChunkingCookieManager();

        options.Cookie.SameSite = SameSiteMode.None;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

        options.SlidingExpiration = false;
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        
        
        options.AccessDeniedPath = "/NotAuthorized";
    })
    .AddOpenIdConnect(options =>
    {
        options.Authority = ssoSettings.IdentityServerUrl;
        options.ClientId = ssoSettings.ClientId;
        options.ClientSecret = ssoSettings.ClientSecret;
        options.ResponseType = OpenIdConnectResponseType.Code;
        options.SaveTokens = true;

        options.RequireHttpsMetadata = false;
        if (builder.Environment.IsDevelopment())
        {
            options.RequireHttpsMetadata = false;
        }

        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("email");
        options.Scope.Add("roles");
        if (ssoSettings.Scopes.Any())
        {
            ssoSettings.Scopes.ToList().ForEach(s => options.Scope.Add(s));
        }

        options.UsePkce = true;

        options.GetClaimsFromUserInfoEndpoint = true;
        
        options.MapInboundClaims = true;

        options.ClaimActions.MapUniqueJsonKey(OpenIddictConstants.Claims.Gender, OpenIddictConstants.Claims.Gender);
        options.ClaimActions.MapUniqueJsonKey("container_id", "container_id");
        options.ClaimActions.MapUniqueJsonKey("container_name", "container_name");
        options.ClaimActions.MapUniqueJsonKey(OpenIddictConstants.Claims.Name, OpenIddictConstants.Claims.Name);
        options.ClaimActions.MapUniqueJsonKey(OpenIddictConstants.Claims.Username, OpenIddictConstants.Claims.Username);
        options.ClaimActions.MapUniqueJsonKey(OpenIddictConstants.Claims.MiddleName, OpenIddictConstants.Claims.MiddleName);
        options.ClaimActions.MapUniqueJsonKey(OpenIddictConstants.Claims.FamilyName, OpenIddictConstants.Claims.FamilyName);
        options.ClaimActions.MapUniqueJsonKey(OpenIddictConstants.Claims.Birthdate, OpenIddictConstants.Claims.Birthdate);
        options.ClaimActions.MapUniqueJsonKey(OpenIddictConstants.Claims.Email, OpenIddictConstants.Claims.Email);
        options.ClaimActions.MapUniqueJsonKey(OpenIddictConstants.Claims.EmailVerified,
            OpenIddictConstants.Claims.EmailVerified);
        options.ClaimActions.MapUniqueJsonKey(OpenIddictConstants.Claims.PhoneNumber, OpenIddictConstants.Claims.PhoneNumber);
        options.ClaimActions.MapUniqueJsonKey(OpenIddictConstants.Claims.PhoneNumberVerified,
            OpenIddictConstants.Claims.PhoneNumberVerified);
        options.ClaimActions.MapUniqueJsonKey(OpenIddictConstants.Claims.Profile, OpenIddictConstants.Claims.Profile);
        options.ClaimActions.MapUniqueJsonKey("isMinor", "isMinor");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = "name",
            RoleClaimType = "role",
            // ValidateIssuer = false,
        };
        
        options.Events.OnTokenValidated = async e =>
        {
            var dbContext = e.HttpContext.RequestServices.GetRequiredService<ApplicationDbContext>();
            var claims = e.Principal.Claims.ToList(); 
            var userIdClaim = claims.FirstOrDefault(c => c.Type == OpenIddictConstants.Claims.Subject)?.Value?? claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            Guid.TryParse(userIdClaim, out var companyId);
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            e.Principal = new ClaimsPrincipal(identity);
            await Task.CompletedTask;
        };

       
    });

builder.Services.AddAuthorization();
builder.Services.AddHealthChecks();

builder.Services.Configure<GzipCompressionProviderOptions>(options =>
    options.Level = System.IO.Compression.CompressionLevel.Fastest);

builder.Services.AddResponseCompression(options => { options.EnableForHttps = true; });
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// app.UseMiddleware<AddCompanyClaimsMiddleware>();


app.MapRazorPages();//.RequireAuthorization();

app.MapControllers();//.RequireAuthorization();

app.MapDefaultControllerRoute();//.RequireAuthorization();

// if (app.Environment.IsDevelopment())
// {
//     app.UseViteDevMiddleware();
// }

app.Run();