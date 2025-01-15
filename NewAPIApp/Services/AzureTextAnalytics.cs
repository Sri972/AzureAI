using Azure;
using Azure.AI.TextAnalytics;

namespace NewAPIApp.Services;

public class AzureTextAnalyticsService
{
    private readonly string _endpoint;
    private readonly string _apiKey;
    private readonly TextAnalyticsClient _client;

    public AzureTextAnalyticsService(IConfiguration configuration)
    {
        _endpoint = configuration["Azure:CognitiveServices:Endpoint"];
        _apiKey = configuration["Azure:CognitiveServices:ApiKey"];

        var credentials = new AzureKeyCredential(_apiKey);
        _client = new TextAnalyticsClient(new Uri(_endpoint), credentials);
    }

    public async Task<string> AnalyzeSentimentAsync(string text)
    {
        var response = await _client.AnalyzeSentimentAsync(text);
        return response.Value.Sentiment.ToString();
    }
}