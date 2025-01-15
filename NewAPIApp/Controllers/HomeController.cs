using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NewAPIApp.Models;
using NewAPIApp.Services;

namespace NewAPIApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AzureTextAnalyticsService _azureService;

    public HomeController(AzureTextAnalyticsService azureService, ILogger<HomeController> logger)
    {
        _azureService = azureService;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AnalyzeSentiment(string userText)
    {
        if (string.IsNullOrEmpty(userText))
        {
            ViewBag.Message = "Please enter some text.";
            return View("Index");
        }

        var sentiment = await _azureService.AnalyzeSentimentAsync(userText);
        ViewBag.Sentiment = sentiment;
        return View("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}