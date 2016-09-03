using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IdentityModel.Claims;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Owin;
using DDAC.Models;
using DDAC.Utils;

namespace DDAC
{
    public partial class Startup
    {
        private static string clientId = ConfigurationManager.AppSettings["ida:ClientId"];
        private static string appKey = ConfigurationManager.AppSettings["ida:ClientSecret"];
        private static string aadInstance = ConfigurationManager.AppSettings["ida:AADInstance"];
        private static string tenantId = ConfigurationManager.AppSettings["ida:TenantId"];
        private static string postLogoutRedirectUri = ConfigurationManager.AppSettings["ida:PostLogoutRedirectUri"];

        public static readonly string Authority = aadInstance + tenantId;

        // This is the resource ID of the AAD Graph API.  We'll need this to request a token to call the Graph API.
        string graphResourceId = "https://graph.windows.net";

        public void ConfigureAuth(IAppBuilder app)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            app.UseOpenIdConnectAuthentication(
               new OpenIdConnectAuthenticationOptions
               {
                   ClientId = ConfigHelper.ClientId,
                    Authority = String.Format(CultureInfo.InvariantCulture, ConfigHelper.AadInstance, ConfigHelper.Tenant), // For Single-Tenant
                    //Authority = ConfigHelper.CommonAuthority, // For Multi-Tenant
                    PostLogoutRedirectUri = ConfigHelper.PostLogoutRedirectUri,

                    // Here, we've disabled issuer validation for the multi-tenant sample.  This enables users
                    // from ANY tenant to sign into the application (solely for the purposes of allowing the sample
                    // to be run out-of-the-box.  For a real multi-tenant app, reference the issuer validation in 
                    // WebApp-MultiTenant-OpenIDConnect-DotNet.  If you're running this sample as a single-tenant
                    // app, you can delete the ValidateIssuer property below.
                    TokenValidationParameters = new System.IdentityModel.Tokens.TokenValidationParameters
                   {
                      // ValidateIssuer = false, // For Multi-Tenant Only
                        RoleClaimType = "roles",
                   },

                   Notifications = new OpenIdConnectAuthenticationNotifications
                   {
                       AuthenticationFailed = context =>
                       {
                           context.HandleResponse();
                           context.Response.Redirect("/Error/ShowError?signIn=true&errorMessage=" + context.Exception.Message);
                           return Task.FromResult(0);
                       }
                   }
               });






            //app.UseOpenIdConnectAuthentication(
            //    new OpenIdConnectAuthenticationOptions
            //    {
            //        ClientId = clientId,
            //        Authority = Authority,
            //        PostLogoutRedirectUri = postLogoutRedirectUri,

            //        Notifications = new OpenIdConnectAuthenticationNotifications()
            //        {
            //            // If there is a code in the OpenID Connect response, redeem it for an access token and refresh token, and store those away.
            //            AuthorizationCodeReceived = (context) =>
            //            {
            //                var code = context.Code;
            //                ClientCredential credential = new ClientCredential(clientId, appKey);
            //                string signedInUserID = context.AuthenticationTicket.Identity.FindFirst(ClaimTypes.NameIdentifier).Value;
            //                AuthenticationContext authContext = new AuthenticationContext(Authority, new ADALTokenCache(signedInUserID));
            //                AuthenticationResult result = authContext.AcquireTokenByAuthorizationCode(
            //                code, new Uri(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Path)), credential, graphResourceId);

            //                return Task.FromResult(0);
            //            }
            //        }
            //    });
        }
    }
}
