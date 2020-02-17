using Microsoft.Azure.Search.Models;
 
namespace BooksCatalogue.Models
{
    public class SearchData
    {
        // The text to search for.
        public string searchText { get; set; }
 
 
        // The list of results.
        public DocumentSearchResult<Book> resultList;
    }
}