using Castle.Components.DictionaryAdapter.Xml;
using DemoApi.Controllers;
using DemoApi.Models;
using DemoApi.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace TestProject
{
    public class StoryControllerTests
    {
        MemoryCacheOptions _cacheOptions;
        MemoryCache _cache = new MemoryCache(new MemoryCacheOptions() { });
        Mock<IStoryRepo> mockStoryRepo;


        public StoryControllerTests()
        {
            mockStoryRepo = new Mock<IStoryRepo>();
        }
        [Fact]
        public void Index_Output_Should_Not_Be_Null()
        {
            StoryController controller = new StoryController(_cache, mockStoryRepo.Object);
            IActionResult op = controller.Index();
            Assert.NotNull(op);
        }
        [Fact]
        public void Fetch_Output_Should_Not_Be_Null()
        {

            var stories = new List<Story>() {
                new Story() { by = "By1", title = "Title1", url = "Url1" },
                new Story() { by = "By2", title = "Title2", url = "Url2" },
                new Story() { by = "By3", title = "Title3", url = "Url3" }
            };
            
            StoryController controller = new StoryController(_cache, mockStoryRepo.Object);
            mockStoryRepo.Setup(x => x.GetStories()).ReturnsAsync(stories);
            var op = controller.Fetch().Result;
            Assert.NotNull(op);
        }


        [Fact]
        public void GetStoryIDs_Output_Should_Not_Be_Null()
        {
            var IDs=new List<int>() {1,2,3,4,5 };
            mockStoryRepo.Setup(x => x.GetStoryIDs()).ReturnsAsync(IDs);
            StoryController controller = new StoryController(_cache, mockStoryRepo.Object);
            List<int> op = controller.GetStoryIDs().Result;
            Assert.NotNull(op);
        }
        [Fact]
        public void GetStoryByID_Output_Should_Not_Be_Null()
        {
            int ID = 39036842;
            Story story = new Story()
            {
                by = "By", title="Title", url="Url"
            };
            mockStoryRepo.Setup(x => x.GetStoryByID(It.IsAny<int>())).ReturnsAsync(story);




            StoryController controller = new StoryController(_cache, mockStoryRepo.Object);
            Story op = controller.GetStoryByID(ID).Result;
            Assert.NotNull(op);
        }
    }
}