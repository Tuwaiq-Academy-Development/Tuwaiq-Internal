using System.Globalization;
using DAL;
using IdentityService.SDK;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OpenIddict.Abstractions;
using StackExchange.Redis;
using AdminUi.Helper;
using AdminUi.Services;

try
{
    
    
    var supportedCultures = new[] { new CultureInfo("ar-EG") };

    var builder = WebApplication.CreateBuilder(args);
    
    // add json from wwwroot/assets.manifest.json
    builder.Configuration.AddJsonFile
    (
        "wwwroot/assets.manifest.json",
        optional: true,
        reloadOnChange: true
    );
    
    // how to retrieve value of file from assets.manifest.json
    
    var root = builder.Environment.ContentRootPath;
    if (!Directory.Exists(Path.Combine(root, "Storage"))) Directory.CreateDirectory(Path.Combine(root, "Storage"));
    if (!Directory.Exists(Path.Combine(root, "Storage", "Files")))
        Directory.CreateDirectory(Path.Combine(root, "Storage", "Files"));
    
    builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

// builder.Services.AddViteServices();

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options
            .UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion("8.0.33"),
                x =>
                    x.CommandTimeout(180).EnableRetryOnFailure())
    );

    builder.Services.AddControllersWithViews()
#if DEBUG
        .AddRazorRuntimeCompilation()
#endif
        ;

    builder.Services.AddRazorPages()
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

    builder.Services.AddAntiforgery(options =>
    {
        options.Cookie.Name = "my-x-12s7";
        options.HeaderName = "my-x-12s7";
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
            // options.Cookie.Path = "/";
            options.LoginPath = "/signin";
            options.LogoutPath = "/signout";
            options.Events.OnSigningOut = async e =>
            {
                // revoke refresh token on sign-out
                await e.HttpContext.RevokeUserRefreshTokenAsync();
            };

            options.CookieManager = new ChunkingCookieManager();

            // options.Cookie.HttpOnly = true;
            options.Cookie.SameSite = SameSiteMode.None;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

            // options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            // options.Cookie.SameSite = SameSiteMode.None;
            // options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            // options.Cookie.SameSite = SameSiteMode.Lax;
            //
            options.SlidingExpiration = false;
            options.ExpireTimeSpan = TimeSpan.FromHours(8);
        })
        .AddOpenIdConnect(options =>
        {
            options.Authority = ssoSettings.IdentityServerUrl;
            options.ClientId = ssoSettings.ClientId;
            options.ClientSecret = ssoSettings.ClientSecret;
            options.ResponseType = OpenIdConnectResponseType.Code;
            options.SaveTokens = true;

            options.RequireHttpsMetadata = false;
            // options.GetClaimsFromUserInfoEndpoint = true;
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

            // options.CallbackPath = "/signin-oidc";
            options.UsePkce = true;


            // options.SignedOutCallbackPath = "/signout-callback-oidc";
            // options.RemoteSignOutPath = "/signout-oidc";
            // options.MapInboundClaims = true;
            options.GetClaimsFromUserInfoEndpoint = true;
            // options.ClaimActions.MapUniqueJsonKey("name",
            //     "name");
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
                //, RoleClaimType = "role"
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
    
    
    
    var redisConfig = builder.Configuration.GetSection("Redis").Value;
    
    builder.Services.AddDistributedMemoryCache();
     builder.Services.AddSingleton<IRedisConnectionFactory>(sp =>
    {
        try
        {
            if (redisConfig != null) return new RedisConnectionFactory(redisConfig);
            return null!;
        }
        catch (Exception ex)
        {
            // Log the exception (consider using a logging framework)
            Console.WriteLine("Error connecting to Redis: " + ex.Message);
            return null!; // Or handle it in a way that suits your application
        }
    });
    
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = redisConfig;
#if DEBUG
        options.InstanceName = "Dev:internals:";
#else
                options.InstanceName = "Landing:";
#endif
    });


    var redis = ConnectionMultiplexer.Connect(redisConfig ?? throw new InvalidOperationException("Redis Configuration is missing"));
    builder.Services
        .AddDataProtection()
        .SetApplicationName("internals")
        .PersistKeysToStackExchangeRedis(redis, 
            
#if DEBUG
            "Dev:internals:DataProtection-Keys"
#else
               "Operation:DataProtection-Keys"
#endif
        )
        .SetDefaultKeyLifetime(TimeSpan.FromDays(14))
        //.PersistKeysToFileSystem(new DirectoryInfo(Path.Join(Environment.CurrentDirectory, "keys")))
        ;


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


    app.MapRazorPages().RequireAuthorization();

    app.MapControllers().RequireAuthorization();

    app.MapDefaultControllerRoute().RequireAuthorization();
    app.MapHealthChecks("/health").AllowAnonymous();

// if (app.Environment.IsDevelopment())
// {
//     app.UseViteDevMiddleware();
// }

    app.Run();

}
catch (Exception ex)
{
    Console.WriteLine(JsonConvert.SerializeObject(ex, Formatting.Indented));
    // Log.Fatal(ex, "Unhandled exception");
}
finally
{
    // Log.Information("Shut down complete");
    // Log.CloseAndFlush();
}