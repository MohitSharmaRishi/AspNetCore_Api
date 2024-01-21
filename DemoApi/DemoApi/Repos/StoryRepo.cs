using DemoApi.Models;
using Newtonsoft.Json;

namespace DemoApi.Repos
{
    public class StoryRepo : IStoryRepo
    {
        public async Task<Story> GetStoryByID(int ID)
        {
            string Url = $"https://hacker-news.firebaseio.com/v0/item/{ID.ToString()}.json?print=pretty";
            Story? story;
            HttpClient client = new HttpClient();
            using (var response = await client.GetAsync(Url))
            {
                var result = await response.Content.ReadAsStringAsync();
                if (result == null) return null;
                try
                {
                    story = JsonConvert.DeserializeObject<Story>(result);
                }
                catch (Exception)
                {
                    return null;
                }
            }
            client.Dispose();
            return story;
        }
        public async Task<List<Story>> GetStories()
        {
            List<int> StoryIDs =await this.GetStoryIDs();
            List<Story> stories = new List<Story>();
            foreach (int storyID in StoryIDs)
            {
                Story story = await this.GetStoryByID(storyID);
                if (story == null) continue;
                stories.Add(story);
            }
            return stories;
        }


        public async Task<List<int>> GetStoryIDs()
        {

            HttpClient _client = new HttpClient();
            var response = await _client.GetAsync("https://hacker-news.firebaseio.com/v0/topstories.json?print=pretty");
            var result = JsonConvert.DeserializeObject<List<int>>(response.Content.ReadAsStringAsync().Result);
            return result;
        }
    }

    public interface IStoryRepo
    {
        Task<Story> GetStoryByID(int ID);
        Task<List<int>> GetStoryIDs();
    Task<List<Story>> GetStories();
    }
}
