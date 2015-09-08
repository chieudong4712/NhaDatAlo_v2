using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceMetadata.API.ViewModels
{
    public class UserViewModel
    {
        #region Personal
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public string Role { get; set; }
        public string About { get; set; }
        public DateTime? Birthday { get; set; }
        #endregion


        #region Contact
        public double? ContactNo { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public double? ZipCode { get; set; }
        public string Website { get; set; }
        public string Fax { get; set; }
        #endregion
    }
}