
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace ResourceMetadata.API.ViewModels
{
    public class SettingViewModel
    {
        public long Id { get; set; }

        public int OrderNumber { get; set; }

        /// <summary>
        /// Key field of setting
        /// </summary>
        /// 
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Text field of setting
        /// </summary>
        [Required]
        public string Text { get; set; }

        /// <summary>
        /// Value field of setting
        /// </summary>
        [Required]
        public string Value { get; set; }


    }
}