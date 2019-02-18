using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Xyzies.SSO.Identity.API.Models.User
{
    public class Profile : BaseProfile
    {
        [Required]
        public string ObjectId { get; set; }

        [Required]
        [StringLength(64)]
        public string UserName { get; set; }

        [Required]
        [StringLength(64)]
        public string Email { get; set; }

        [StringLength(64)]
        public string CompanyName { get; set; }
    }
}
