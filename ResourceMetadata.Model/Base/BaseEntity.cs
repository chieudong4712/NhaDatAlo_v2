using System;

namespace ResourceMetadata.Model.Base
{
    public interface IBaseEntity
    {
        long Id { get; set; }
        DateTime CreatedDateTime { get; set; }
        DateTime LastUpdatedDateTime { get; set; }
        string CreatedBy { get; set; }
        string LastUpdatedBy { get; set; }
        string Status { get; set; }
        string Attribute { get; set; }
    }
    public class BaseEntity : IBaseEntity
    {
        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        public long Id { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastUpdatedDateTime { get; set; }
        public string CreatedBy { get; set; }
        public string LastUpdatedBy { get; set; }
        public string Status { get; set; }
        public string Attribute { get; set; }
        


        public override bool Equals(object obj)
        {
            return Equals(obj as BaseEntity);
        }

        private static bool IsTransient(BaseEntity obj)
        {
            return obj != null && Equals(obj.Id, default(long));
        }

        private Type GetUnproxiedType()
        {
            return GetType();
        }

        public virtual bool Equals(BaseEntity other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (!IsTransient(this) &&
                !IsTransient(other) &&
                Equals(Id, other.Id))
            {
                var otherType = other.GetUnproxiedType();
                var thisType = GetUnproxiedType();
                return thisType.IsAssignableFrom(otherType) ||
                        otherType.IsAssignableFrom(thisType);
            }

            return false;
        }

        public override int GetHashCode()
        {
            if (Equals(Id, default(int)))
                return base.GetHashCode();
            return Id.GetHashCode();
        }

        public static bool operator ==(BaseEntity x, BaseEntity y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(BaseEntity x, BaseEntity y)
        {
            return !(x == y);
        }
    }

    
}
