using DemoApi.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DemoApi.Repos
{
    public class StoryRepo : IStoryRepo
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>This method will return Single Story record for the given Story number / ID</returns>
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

        /// <summary>
        ///  
        /// </summary>
        /// <returns>This Method will return 200 Stories from Api</returns>
        public async Task<List<Story>> GetStories()
        {
            HttpClient client = new HttpClient();
            List<int> StoryIDs =await this.GetStoryIDs();
            List<Task<string>> tasks = new List<Task<string>>();
            List<Story> stories = new List<Story>();

            foreach (var ID in StoryIDs.Take(200))
            {
                async Task<string> func()
                {
                    var response = await client.GetAsync($"https://hacker-news.firebaseio.com/v0/item/{ID.ToString()}.json?print=pretty");
                    return await response.Content.ReadAsStringAsync();
                }

                tasks.Add(func());
            }
            await Task.WhenAll(tasks);

            var postResponses = new List<string>();

            foreach (var t in tasks)
            {
                var postResponse = await t; //t.Result would be okay too.
                Story story;
                try
                {
                    story = JsonConvert.DeserializeObject<Story>(postResponse);
                }
                catch (Exception)
                {
                    continue;
                }
                stories.Add(story);
                
            }


            return stories;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns>This method will return numerical IDs of stories
        /// </returns>
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
