using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace ResourceMetadata.API.ViewModels
{
    public class ChangePasswordModel
    {
        [Required]
        public string OldPassword { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password should contain at least 6 characters")]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Password and confirm password should match")]
        public string ConfirmPassword { get; set; }
    }
}