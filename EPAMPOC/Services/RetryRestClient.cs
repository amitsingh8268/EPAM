using EPAM_POC.Contract;
using ThirdParty;

namespace EPAM_POC.Services
{
    public class RetryRestClient : IRetryRestClient
    {
        private readonly IRestClient _originalRestClient;
        private readonly ILogger<RetryRestClient> _logger;
        private readonly int _retryCount;
        private readonly TimeSpan _retryDelay;


        public RetryRestClient(IRestClient originalRestClient, ILogger<RetryRestClient> logger, int retryCount = 3, TimeSpan? retryDelay = null)
        {
            _originalRestClient = originalRestClient;
            _logger = logger;
            _retryCount = retryCount;
            _retryDelay = retryDelay ?? TimeSpan.FromSeconds(1); // Default retry delay of 1 second
        }


        public async Task<TModel> Get<TModel>(string url)
        {
            return await Retry(() => _originalRestClient.Get<TModel>(url));
        }

        public async Task<TModel> Put<TModel>(string url, TModel model)
        {
            return await Retry(() => _originalRestClient.Put(url, model));
        }

        public async Task<TModel> Post<TModel>(string url, TModel model)
        {
            return await Retry(() => _originalRestClient.Post(url, model));
        }

        public async Task<TModel> Delete<TModel>(int id)
        {
            return await Retry(() => _originalRestClient.Delete<TModel>(id));
        }

        private async Task<TModel> Retry<TModel>(Func<Task<TModel>> func)
        {
            int retriesLeft = _retryCount;

            while (true)
            {
                try
                {
                    return await func();
                }
                catch (System.Net.WebException ex)
                {
                    if (retriesLeft <= 0)
                    {
                        _logger.LogError(ex, "Retry attempts exhausted.");
                        throw; // Re-throw the exception after all attempts
                    }

                    _logger.LogError(ex, "Retry attempt {RetryCount}.", _retryCount - retriesLeft + 1);
                    await Task.Delay(_retryDelay);
                    retriesLeft--;
                }
            }
        }
    }
}
