namespace DNSChanger.Models
{
    public class DNSOption
    {
        public string Name { get; set; }
        public string Preferred { get; set; }
        public string Alternate { get; set; }

        public DNSOption(string name, string preferred, string alternate)
        {
            Name = name;
            Preferred = preferred;
            Alternate = alternate;
        }
    }
}