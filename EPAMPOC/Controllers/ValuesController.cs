using Microsoft.AspNetCore.Mvc;
using ThirdParty;

namespace EPAMPOC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private IRestClient _3rdPartyRestClient;
        private ILogger _logger;

        public ValuesController(IRestClient restClient, ILogger logger)
        {
            _3rdPartyRestClient = restClient;
            _logger = logger;
        }

        [HttpGet("{url}", Name = "GetThirdPartyData")]
        public Task<TModel> GetThirdPartyData<TModel>(string url)
        {
            return _3rdPartyRestClient.Get<TModel>(url);
        }
    }
}
