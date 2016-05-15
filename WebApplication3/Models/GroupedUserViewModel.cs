using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace WebApplication3.Models
{
    public class GroupedUserViewModel
    {
        public List<UserViewModel> UsersWholeSale { get; set; }
        public List<UserViewModel> UsersRetail { get; set; }
    }

    public class UserViewModel
    {
        public string Username { get; set; }
        public string Roles { get; set; }
    }
}