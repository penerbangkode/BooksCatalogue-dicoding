using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace BooksCatalogue.Models
{
    public class Book
    {
    [JsonPropertyName("id")]
    public int Id { get; set; }
 
    [IsSortable, IsSearchable]
    [JsonPropertyName("title")]
    [StringLength(60, MinimumLength = 3)]
    [Required]
    public string Title { get; set; }
 
    [IsFilterable, IsFacetable, IsSearchable]
    [JsonPropertyName("author")]
    [Required]
    public string Author { get; set; }
 
    [JsonPropertyName("synopsis")]
    [Required]
    public string Synopsis { get; set; }
 
    [IsFilterable, IsSortable, IsFacetable]
    [Display(Name = "Release Year")]
    [JsonPropertyName("releaseYear")]
    [Required]
    public int ReleaseYear { get; set; }
 
    [Display(Name = "Cover URL")]
    [JsonPropertyName("coverURL")]
    [Required]
    public string CoverURL { get; set; }
 
    [JsonPropertyName("reviews")]
    public ICollection<Review> Reviews { get; set; }
    }
}