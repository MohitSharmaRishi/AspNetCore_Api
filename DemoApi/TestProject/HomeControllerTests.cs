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
        private readonly Mock<IMemoryCache> _cache=new Mock<IMemoryCache>();
        public HomeControllerTests()
        {
            //_cacheOptions = new MemoryCacheOptions()
            //{
                 
            //};
            //_cache =new MemoryCache(_cacheOptions);
        }
        [Fact]
        public void Check_Index_Output()
        {

            HomeController controller = new HomeController(_cache.Object);
            IActionResult op = controller.Index();
            Assert.NotNull(op);
        }
        [Fact]
        public void Check_Fetch_Output()
        {

            HomeController controller = new HomeController(_cache.Object);
            IActionResult op = controller.Index();
            Assert.NotNull(op);
        }


        [Fact]
        public void GetDataFromApi_Output() { 
        HomeController controller = new HomeController(_cache.Object);
            List<Story> op = controller.GetDataFromApi().Result;
            Assert.NotNull(op);
        }
    }
}