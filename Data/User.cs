using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Api.Data
{
    // Add profile data for application users by adding properties to the User class
    public class User : IdentityUser
    {
        // Add custom properties to your users
        public string FullName { get; set; }
        public string BillingAdress { get; set; }
        public string DefaultShippingAddress { get; set; }
        public string Country { get; set; }
        public string MailToken { get; set; }

        // Setup one to one relation

        public virtual UserSettings Settings { get; set; }
        public virtual UserGDPR GDPR { get; set; }
    }

    public class UserSettings
    {
        [Key]
        public string Id { get; set; }
        public bool DarkMode { get; set; }

        // assign foreign key (which is the primary key of the parent table, in this case (Id) of user)
        [ForeignKey("Id")]
        public virtual User User { get; set; }


    }

    public class UserGDPR
    {
        [Key]
        public string Id { get; set; }
        [PersonalData]
        public bool UseMyData { get; set; }

        [ForeignKey("Id")]
        public virtual User User { get; set; }
    }
    
}
