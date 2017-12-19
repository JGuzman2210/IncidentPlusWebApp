using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using System.Security.Claims;
using System.Web.Http;
using IncidentPlus.Data.UserRepository;
using IncidentPlus.Entity.Entities;
using System.Collections.Generic;
using System.Web.Cors;

[assembly: OwinStartup(typeof(IncidentPlus.API.Startup))]

namespace IncidentPlus.API
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);//Enable Cors
           
            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(2),
                Provider = new MyCustomProvider()
            });

            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            HttpConfiguration config = new HttpConfiguration();
            app.UseWebApi(config);
            WebApiConfig.Register(config);
        }
    }

    public class MyCustomProvider : OAuthAuthorizationServerProvider
    {
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            return base.TokenEndpoint(context);
        }
        public override Task TokenEndpointResponse(OAuthTokenEndpointResponseContext context)
        {
           var lista = context.Identity.Claims;
    
            foreach (var item in lista)
            {
                context.AdditionalResponseParameters.Add(item.Type, item.Value);
            }

            return base.TokenEndpointResponse(context);
        }
        public override Task ValidateTokenRequest(OAuthValidateTokenRequestContext context)
        {
            return base.ValidateTokenRequest(context);
        }
        public override Task MatchEndpoint(OAuthMatchEndpointContext context)
        {//Llamado por cada request
            //context.is
            return base.MatchEndpoint(context);
        }
        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            return base.GrantRefreshToken(context);
        }
        public override Task GrantAuthorizationCode(OAuthGrantAuthorizationCodeContext context)
        {
            return base.GrantAuthorizationCode(context);
        }
        public override Task AuthorizeEndpoint(OAuthAuthorizeEndpointContext context)
        {
            return base.AuthorizeEndpoint(context);
        }

        public override Task AuthorizationEndpointResponse(OAuthAuthorizationEndpointResponseContext context)
        {
            return base.AuthorizationEndpointResponse(context);
        }

        public override Task GrantClientCredentials(OAuthGrantClientCredentialsContext context)
        {
            return base.GrantClientCredentials(context);
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            context.Validated();
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            
            var authIdentity = new ClaimsIdentity(context.Options.AuthenticationType);
            var _userRepo = UserRepository.NewInstance();

            var userTemp = _userRepo.ValidateCredential(new User()
            {
                UserName = context.UserName,
                Password = context.Password
            });

            if(userTemp != null)
            {
                authIdentity.AddClaim(new Claim(ClaimTypes.Role,userTemp.Rol.Name));
                authIdentity.AddClaim(new Claim("role", userTemp.Rol.Name));
                authIdentity.AddClaim(new Claim("userName",userTemp.UserName));
                authIdentity.AddClaim(new Claim("name", userTemp.Name));
                authIdentity.AddClaim(new Claim("email", userTemp.Email));
                
                context.Validated(authIdentity);
            }else
            {
                context.SetError("invalid", "Please username and password is invalid");
            }

            //return base.GrantResourceOwnerCredentials(context);
        }
    }
   
}
