using DemoApi.Models;
using DemoApi.Repos;
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
        private readonly IStoryRepo _storyRepo;
        public StoryController(IMemoryCache cache, IStoryRepo storyRepo)
        {
            _storyRepo = storyRepo;
            _cache =  cache;
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
            return await _storyRepo.GetStories();
        }
        [NonAction]
        public async Task<List<int>> GetStoryIDs()
        {

            return await _storyRepo.GetStoryIDs();

        }


        [HttpGet("fetch/{ID}")]
        public async Task<Story> GetStoryByID(int ID)
        {
            return await _storyRepo.GetStoryByID(ID);
            
        }

    }
}
