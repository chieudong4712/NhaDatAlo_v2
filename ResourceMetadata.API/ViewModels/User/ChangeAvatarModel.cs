using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace ResourceMetadata.API.ViewModels
{
    public class ChangeAvatarModel
    {
        [Required]
        public string Avatar { get; set; }

        
    }
}