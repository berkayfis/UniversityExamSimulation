using Newtonsoft.Json;

namespace UniversityExamSimulation.Core.Models
{
    public class WikipediaResponse
    {
        public Item[] Items { get; set; }
    }
    public class Item
    {
        public Article[] Articles { get; set; }
    }
    public class Article
    {
        [JsonProperty("article")]
        public string Name { get; set; }
        public int Views { get; set; }
        public int Rank { get; set; }
    }
}
