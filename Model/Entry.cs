namespace Model
{

    public class Entry
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string Category { get; set; }

        public override string ToString()
        {
            return Name + " " + Password + " " + Category;
        }
    }
}
