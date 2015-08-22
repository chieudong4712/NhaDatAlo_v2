using ResourceMetadata.Model.Base;
using System;


namespace ResourceMetadata.Model
{
    public class UserProfile: BaseEntity
    {
        public UserProfile()
        {
        }

        public DateTime? Birthday { get; set; }

        public bool? Gender { get; set; }

        public string About { get; set; }

        public string UserId { get; set; }

        //public virtual ApplicationUser User { get; set; }

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
