using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;

namespace NewAPIApp.Services
{
    public class SearchService
    {
        private readonly string _searchServiceEndpoint = "https://aisearch972.search.windows.net";
        private readonly string _indexName = "football-index-new";
        private readonly string _apiKey = "B5bVxjOrGQouM7GoinLAauEPG2aPxlMlSNAnADJ3y5AzSeDAN4dB";

        public async Task<string> SearchAsync(string query)
        {
            var client = new SearchClient(new Uri(_searchServiceEndpoint), _indexName, new AzureKeyCredential(_apiKey));

            // Perform search query
            var searchResults = await client.SearchAsync<SearchResult>(query);
            var resres = searchResults.Value.GetResultsAsync();
            SearchResult searchRes = new SearchResult();
            // Return a string with the search results
            var resultString = "Search Results:\n";

            await foreach (var result in searchResults.Value.GetResultsAsync())
            {
                searchRes.Content = result.Document.Content;
                resultString += $"{result.Document.Content}\n";
            }

            return string.IsNullOrEmpty(resultString) ? "No results found" : resultString;
        }

        public async Task IndexPdfContentAsync(string filePath)
        {
            // Extract text from the PDF and create documents to be indexed
            var extractedText = ExtractTextFromPdf(filePath);
            var documentId = Path.GetFileNameWithoutExtension(filePath); 
            var client = new SearchClient(new Uri(_searchServiceEndpoint), _indexName, new AzureKeyCredential(_apiKey));

            var batch = IndexDocumentsBatch.Upload(new List<SearchResult>
            {
                new SearchResult
                {
                    Id = documentId,  
                    Content = extractedText
                }
            });

            await client.IndexDocumentsAsync(batch);
        }

        private string ExtractTextFromPdf(string filePath)
        {
            // Implement PDF text extraction (similar to the previous example with PdfSharpCore)
            var text = "Sample extracted text from PDF..."; // This should be extracted from the PDF file

            return text;
        }
    }

    public class SearchResult
    {
        public string Id { get; set; }
        public string Content { get; set; }
    }
}
