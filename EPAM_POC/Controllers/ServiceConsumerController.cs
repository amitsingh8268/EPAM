using Microsoft.AspNetCore.Mvc;
using ThirdParty;

namespace EPAM_POC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceConsumerController : ControllerBase
    {
        private readonly IRestClient _3rdParyClient;
        public ServiceConsumerController(IRestClient thirdPartyClient)
        {
            _3rdParyClient = thirdPartyClient;
        }

        [HttpGet]
        public async Task<IActionResult> GetDataFromThirdParty(string url)
        {
            try
            {
                var data = await _3rdParyClient.Get<string>(url);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
