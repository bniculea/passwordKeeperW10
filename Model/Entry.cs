using SQLite.Net.Attributes;

namespace Model
{

    public class Entry
    {
        [PrimaryKey, AutoIncrement, Column("Id")]
        public int Id { get; set; }
        public string Name { get;  set; }
        public string Password { get;  set; }
        public string Category { get;  set; }

        public override string ToString()
        {
            return Name + " " + Password + " " + Category;
        }
    }
}
