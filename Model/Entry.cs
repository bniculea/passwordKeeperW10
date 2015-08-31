using System;
using SQLite.Net.Attributes;

namespace Model
{

    public class Entry
    {
        [PrimaryKey, AutoIncrement, Column("Id")]
        public int Id { get; set; }
        [Column("Name")]
        public string Name { get;  set; }
        [Column("Password")]
        public string Password { get;  set; }
        [Column("Category")]
        public string Category { get;  set; }

        public override string ToString()
        {
            return Name + " " + Password + " " + Category;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Entry entry = obj as Entry;
            if ((Object)entry== null) return false;
            return (Name.Equals(entry.Name) && Password.Equals(entry.Password) && Category.Equals(entry.Category));
        }

        protected bool Equals(Entry other)
        {
            return Id == other.Id && string.Equals(Name, other.Name) && string.Equals(Password, other.Password) && string.Equals(Category, other.Category);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id;
                hashCode = (hashCode*397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Password != null ? Password.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Category != null ? Category.GetHashCode() : 0);
                return hashCode;
            }
        }
        public static bool operator ==(Entry e1, Entry e2)
        {
            if (object.ReferenceEquals(e1, e2)) return true;
            if (((object)e1 == null) || ((object)e2 == null))
            {
                return false;
            }
            return e1.Name.Equals(e2.Name) && e1.Password.Equals(e2.Password) && e1.Category.Equals(e2.Category);
        }
        public static bool operator !=(Entry e1, Entry e2)
        {
            return !(e1 == e2);
        }

    }
}
