using ThirdParty;

public class RetryRestClient : IRestClient
{
    private readonly ILogger<RetryRestClient> _logger;
    private readonly int _retryCount;
    private readonly TimeSpan _retryDelay;
    private readonly HttpClient _httpClient;

    public RetryRestClient(HttpClient httpClient, ILogger<RetryRestClient> logger, int retryCount = 3, TimeSpan? retryDelay = null)
    {
        _httpClient = httpClient;
        _logger = logger;
        _retryCount = retryCount;
        _retryDelay = retryDelay ?? TimeSpan.FromSeconds(1); // Default retry delay of 1 second
    }

    public async Task<TModel> Get<TModel>(string url)
    {
        return await Retry(async () => await ExecuteRequest<TModel>(() => _httpClient.GetAsync(url)));
    }

    public async Task<TModel> Put<TModel>(string url, TModel model)
    {
        return await Retry(async () => await ExecuteRequest<TModel>(() => _httpClient.PutAsJsonAsync(url, model)));
    }

    public async Task<TModel> Post<TModel>(string url, TModel model)
    {
        return await Retry(async () => await ExecuteRequest<TModel>(() => _httpClient.PostAsJsonAsync(url, model)));
    }

    public async Task<TModel> Delete<TModel>(int id)
    {
        var url = $"your_api_endpoint_here/{id}";
        return await Retry(async () => await ExecuteRequest<TModel>(() => _httpClient.DeleteAsync(url)));
    }

    private async Task<TModel> ExecuteRequest<TModel>(Func<Task<HttpResponseMessage>> requestFunc)
    {
        //throw new WebException();
        var response = await requestFunc();

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<TModel>();
        }
        else
        {
            throw new HttpRequestException($"HTTP request failed with status code {response.StatusCode}.");
        }
    }

    private async Task<TModel> Retry<TModel>(Func<Task<TModel>> func)
    {
        int retriesLeft = _retryCount;

        while (true)
        {
            try
            {
                //throw new WebException();
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