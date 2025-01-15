using Microsoft.AspNetCore.Mvc;
using NewAPIApp.Services;

namespace NewAPIApp.Controllers
{
    public class FantasyController : Controller
    {
        private readonly SearchService _searchService;

        public FantasyController(SearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadAndSearchPdf(IFormFile file, string query)
        {
            if (file != null && file.Length > 0)
            {
                // Save the uploaded PDF file to a temporary location
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Index the content of the PDF into Azure Cognitive Search
                await _searchService.IndexPdfContentAsync(filePath);

                // Perform the search using the provided query
                var searchResults = await _searchService.SearchAsync(query);

                return View("SearchResults", searchResults);
            }

            return View("Index");
        }
    }
}