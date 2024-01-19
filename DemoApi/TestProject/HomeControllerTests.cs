using DemoApi.Controllers;
using DemoApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace TestProject
{
    public class HomeControllerTests
    {
        MemoryCacheOptions _cacheOptions;
        MemoryCache _cache = new MemoryCache(new MemoryCacheOptions() { });
        public HomeControllerTests()
        {
            
        }
        [Fact]
        public void Index_Output_Should_Not_Be_Null()
        {

            StoryController controller = new StoryController(_cache);
            IActionResult op = controller.Index();
            Assert.NotNull(op);
        }
        [Fact]
        public void Fetch_Output_Should_Not_Be_Null()
        {
            
            StoryController controller = new StoryController(_cache);
            var op = controller.Fetch().Result;
            Assert.NotNull(op);
        }


        [Fact]
        public void GetDataFromApi_Output_Should_Not_Be_Null()
        {
            StoryController controller = new StoryController(_cache);
            List<Story> op = controller.GetStories().Result;
            Assert.NotNull(op);
        }
        [Fact]
        public void FetchSingleRecord_Output_Should_Not_Be_Null()
        {
            int ID = 39036842;
            StoryController controller = new StoryController(_cache);
            Story op = controller.FetchSingleRecord(ID).Result;
            Assert.NotNull(op);
        }
    }
}