using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Components.Authorization;
using Timereg.Models;

namespace Timereg
{
    public class SecurityService
    {
        private readonly NavigationManager navigationManager;
        private readonly AuthenticationStateProvider authenticationStateProvider;

        public SecurityService(NavigationManager navigationManager, AuthenticationStateProvider authenticationStateProvider)
        {
            this.navigationManager = navigationManager;
            this.authenticationStateProvider = authenticationStateProvider;
        }

        private ApplicationUser user;

        public ApplicationUser User
        {
            get
            {
                var name = Principal.FindFirstValue("name");

                return user = user ?? new ApplicationUser { Name = name };
            }
        }

        public ClaimsPrincipal Principal
        {
            get
            {
                return authenticationStateProvider.GetAuthenticationStateAsync().Result.User;
            }
        }

        public bool IsInRole(params string[] roles)
        {
            if (roles.Contains("Everybody"))
            {
                return true;
            }

            if (!IsAuthenticated())
            {
                return false;
            }

            if (roles.Contains("Authenticated"))
            {
                return true;
            }

            return roles.Any(role => Principal.IsInRole(role));
        }

        public bool IsAuthenticated()
        {
            return Principal.Identity.IsAuthenticated;
        }

        public async Task Logout()
        {
            navigationManager.NavigateTo("Account/Logout", true);
        }

        public async Task Login()
        {
            navigationManager.NavigateTo("Account/Login", true);
        }
    }
}
