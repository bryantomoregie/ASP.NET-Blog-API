using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace HatchwaysBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class PingController : ControllerBase
    {
        public async Task<ActionResult<string>> GetPings()
        {
            var success = new Dictionary<string, string>(){
                        {"success", "true"} };

            JsonResult jsonResult = new JsonResult(success);
            jsonResult.StatusCode = 200;
            return jsonResult;
        }

    }
}


