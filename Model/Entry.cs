namespace Model
{

    public class Entry
    {
        public string Name { get; private set; }
        public string Password { get; private set; }
        public string Category { get; private set; }

        public Entry(string name, string password, string category)
        {
            Name = name;
            Password = password;
            Category = category;
        }

        public override string ToString()
        {
            return Name + " " + Password + " " + Category;
        }
    }
}
