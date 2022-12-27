using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Timereg.Models.Timeregdata;

namespace Timereg.Models
{
    public partial class ApplicationUser
    {
        public string Name { get; set; }
    }
    public static class UserProfile
    {

        public static async Task<int> GetUserId(SecurityService security, TimeregdataService timeregdata)
        {
            //var authState = await security.GetAuthenticationStateAsync();
            //var user = authState.User;
            if (security != null && security.IsAuthenticated())
            {
                var profile = new Employee
                {
                    username = security.User.Name
                };

                foreach (var claim in security.Principal.Claims)
                {

                    if (claim.Type.Contains("emailaddress")) profile.email = claim.Value;
                  
                }

                var p = await timeregdata.GetEmployees();
                if (p != null && p.Any())
                {
                    var usr = p.ToList().FirstOrDefault(u => u.username == profile.username);
                    if(usr!=null)
                    return usr.userid;
                }
                var i = await timeregdata.CreateEmployee(profile);
                return i.userid;
            }
            else return 0;


        }
    }
}
