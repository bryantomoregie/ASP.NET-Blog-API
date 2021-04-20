using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HatchwaysBlog.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq.Dynamic.Core;
using MoreLinq;
using System.Net;

namespace HatchwaysBlog.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        HttpClient client = new HttpClient();
      
        public async Task<ActionResult<string>> GetPosts(string tag, string sortBy="id", string direction="asc")
        {
            if (tag == null)
            {
                return CreateError("Tags parameter is required");
            }
            if (direction != "asc" && direction != "desc")
            {
                return CreateError("direction parameter is invalid");
            }
            if (sortBy != "id" && sortBy != "reads" && sortBy != "likes" && sortBy != "popularity")
            {
                return CreateError("sortBy parameter is invalid");
            }

            var postList = await GetPostsItems(tag);

            if (postList.Count == 0)
            {
                return CreateError("A Tag item does not exist");

            }
            List<Post> posts = postList.AsQueryable().OrderBy($"{sortBy} {direction}").ToList();
            var distinctList = posts.DistinctBy(x => x.id).ToList();

            Dictionary<string, List<Post>> obj = new Dictionary<string, List<Post>>();
            obj.Add("posts", distinctList);


            var json = JsonConvert.SerializeObject(obj);
          
            return json;
        }

        private async Task<List<Post>> GetPostsItems(string tag)
        {
            List<Post> allPosts = new List<Post>();

            string[] namesArray = tag.Split(',');
            foreach (var item in namesArray)
            {
                var jsonString = await client.GetStringAsync($"https://api.hatchways.io/assessment/blog/posts?tag={item}");
                dynamic response = JsonConvert.DeserializeObject(jsonString);
                List<Post> posts = response.posts.ToObject<List<Post>>();
                if (posts.Count == 0)
                {
                    return posts;
                }
                if (allPosts.Count > 0 ) {
                    allPosts.AddRange(posts);
                }
                else
                {
                    allPosts = posts;
                }
            }

            return allPosts;         
        }

        private JsonResult CreateError(string errorMessage)
        {
            var error = new Dictionary<string, string>(){
                        {"error", errorMessage} };

            JsonResult jsonResult = new JsonResult(error);
            jsonResult.StatusCode = 400;
            return jsonResult;
        }
    }
}



