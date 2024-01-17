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
    public class HomeController : ControllerBase
    {
        HttpClient _client;
        readonly IMemoryCache _cache;
        MemoryCacheEntryOptions _cacheOptions;
        
        public HomeController(IMemoryCache cache)
        {
            _cache=  cache;
            _cacheOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddHours(10)
            };
        }
        [Route("")]
        public IActionResult Index()
        {
            return Ok("Server Is Online");

        }
        [Route("fetch")]
        public async Task<IActionResult> Fetch()
        {
            List<Story> stories;
            bool HasData = _cache.TryGetValue( "stories", out stories);
            if(!HasData)
            {
                stories = await GetDataFromApi();
                _cache.Set("stories", stories, _cacheOptions);
            }
            else
            {
                        
                stories =(List<Story>) _cache.Get("stories");
            }

            
            return Ok(stories);
           
        }
        [NonAction]
        public async Task<List<Story>> GetDataFromApi()
        {
            List<Story> stories = new List<Story>();
            _client = new HttpClient();
            var response = _client.GetAsync("https://hacker-news.firebaseio.com/v0/topstories.json?print=pretty").Result;
            var result = JsonConvert.DeserializeObject<List<int>>(response.Content.ReadAsStringAsync().Result);
            if (result == null || result.Count==0) return null;

            foreach (var item in result.Take(200).ToList())
            {
                response = _client.GetAsync($"https://hacker-news.firebaseio.com/v0/item/{item}.json?print=pretty").Result;
                Story story = JsonConvert.DeserializeObject<Story>(response.Content.ReadAsStringAsync().Result);
                stories.Add(story);
            }

            _client.Dispose();
            return stories;
        }

    }
}
