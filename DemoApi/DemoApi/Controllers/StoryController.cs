using DemoApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace DemoApi.Controllers
{
    [Route("")]
    [ApiController]
    public class StoryController : ControllerBase
    {
        HttpClient _client;
        readonly IMemoryCache _cache;
        MemoryCacheEntryOptions _cacheOptions;
        
        public StoryController(IMemoryCache cache)
        {
            _cache=  cache;
            _cacheOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddHours(10)
            };
        }
        
        [HttpGet("")]
        public IActionResult Index()
        {
            return Ok("Server Is Online");

        }
        
        [HttpGet("fetch")]
        public async Task<IActionResult> Fetch()
        {
            List<Story> stories;
            bool HasData = _cache.TryGetValue( "stories", out stories);
            if(!HasData)
            {
                stories = await GetStories();
                _cache.Set("stories", stories, _cacheOptions);
            }
            else
            {
                        
                stories =(List<Story>) _cache.Get("stories");
            }

            
            return Ok(stories);
           
        }
        [NonAction]
        public async Task<List<Story>> GetStories()
        {
            List<Story> stories = new List<Story>();
            _client = new HttpClient();
            var response = _client.GetAsync("https://hacker-news.firebaseio.com/v0/topstories.json?print=pretty").Result;
            var result = JsonConvert.DeserializeObject<List<int>>(response.Content.ReadAsStringAsync().Result);
            if (result == null || result.Count==0) return stories;

            foreach (var item in result.Take(200).ToList())
            {
                Story story = await FetchSingleRecord(item);
                if (story == null) continue;
                stories.Add(story);
            }
            _client.Dispose();
            return stories;
        }
        [HttpGet("fetch/{ID}")]
        public async Task<Story> FetchSingleRecord(int ID)
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

    }
}
