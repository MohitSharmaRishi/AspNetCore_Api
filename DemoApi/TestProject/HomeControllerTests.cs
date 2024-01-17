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
        public void Check_Index_Output()
        {

            HomeController controller = new HomeController(_cache);
            IActionResult op = controller.Index();
            Assert.NotNull(op);
        }
        [Fact]
        public void Check_Fetch_Output()
        {
            
            HomeController controller = new HomeController(_cache);
            var op = controller.Fetch().Result;
            Assert.NotNull(op);
        }


        [Fact]
        public void GetDataFromApi_Output() { 
        HomeController controller = new HomeController(_cache);
            List<Story> op = controller.GetDataFromApi().Result;
            Assert.NotNull(op);
        }
    }
}