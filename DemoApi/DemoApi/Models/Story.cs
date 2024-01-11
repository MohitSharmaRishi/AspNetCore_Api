namespace DemoApi.Models
{
    public class Story
    {
        //public string by { get; set; }
        //public int descendants { get; set; }
        public int id { get; set; }
        //public List<int>? kids { get; set; }
        //public int score { get; set; }
        public int time { get; set; }
        public string title { get; set; } = string.Empty;
        //public string type { get; set; } = string.Empty;
        public string url { get; set; } = string.Empty;
    }

}
